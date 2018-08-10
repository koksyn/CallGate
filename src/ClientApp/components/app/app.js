import Vue from 'vue';
import VueRouter from 'vue-router';
import VueEvents from 'vue-events';
import VueLoadingOverlay from 'vue-loading-overlay';
import 'vue-loading-overlay/dist/vue-loading.min.css';
import NotificationService from "../../services/ui/NotificationService";
import SocketManager from "../../services/socket/SocketManager";
import AccountStore from "../../stores/AccountStore";
import GroupStore from "../../stores/GroupStore";
import UserStore from "../../stores/UserStore";
import ChannelStore from "../../stores/ChannelStore";
import ChatStore from "../../stores/ChatStore";
import TypeCheckService from "../../services/utils/TypeCheckService";
import Logout from "../../mixins/Logout";
import SidebarService from "../../services/ui/SidebarService";
import ThemeService from "../../services/ui/ThemeService";
import DialogService from "../../services/ui/DialogService";
import EventStore from "../../stores/EventStore";
import DatetimeFormatter from "../../mixins/DatetimeFormatter";
import GroupCreatedEventHandler from "../../handlers/GroupCreatedEventHandler";
import GroupEditedEventHandler from "../../handlers/GroupEditedEventHandler";
import UserRemovedFromGroupEventHandler from "../../handlers/UserRemovedFromGroupEventHandler";
import UserAddedToGroupEventHandler from "../../handlers/UserAddedToGroupEventHandler";
import MemberRoleInGroupGrantedEventHandler from "../../handlers/MemberRoleInGroupGrantedEventHandler";
import AdminRoleInGroupGrantedEventHandler from "../../handlers/AdminRoleInGroupGrantedEventHandler";
import ChannelCreatedEventHandler from "../../handlers/ChannelCreatedEventHandler";
import ChatCreatedEventHandler from "../../handlers/ChatCreatedEventHandler";
import UserAddedToChannelEventHandler from "../../handlers/UserAddedToChannelEventHandler";
import UserRemovedFromChannelEventHandler from "../../handlers/UserRemovedFromChannelEventHandler";
import UserAddedToChatEventHandler from "../../handlers/UserAddedToChatEventHandler";
import UserRemovedFromChatEventHandler from "../../handlers/UserRemovedFromChatEventHandler";
import HttpErrorHandler from "../../mixins/HttpErrorHandler";
import SocketConnector from "../../mixins/SocketConnector";
import GroupActivator from "../../mixins/GroupActivator";
import AuthenticationService from "../../services/AuthenticationService";
import SocketReactor from "../../mixins/SocketReactor";
import ThemeChanger from "../../mixins/ThemeChanger";

Vue.use(VueRouter);
Vue.use(VueEvents);
Vue.use(VueLoadingOverlay);

export default {
    mixins: [
        // sockets
        SocketReactor, SocketConnector,

        // stores
        AccountStore, GroupStore, UserStore,
        ChannelStore, ChatStore, EventStore,
        
        // utils
        GroupActivator, HttpErrorHandler,
        Logout, DatetimeFormatter,
        ThemeChanger,

        // event handlers
        GroupCreatedEventHandler,
        GroupEditedEventHandler,
        UserAddedToGroupEventHandler,
        UserRemovedFromGroupEventHandler,
        MemberRoleInGroupGrantedEventHandler,
        AdminRoleInGroupGrantedEventHandler,
        ChannelCreatedEventHandler,
        UserAddedToChannelEventHandler,
        UserRemovedFromChannelEventHandler,
        ChatCreatedEventHandler,
        UserAddedToChatEventHandler,
        UserRemovedFromChatEventHandler
    ],
    data: function () {
        return {
            dashboardViewMode: true
        }
    },
    beforeMount: function () {
        this.switchViewMode(this.$route.matched);
        
        if (this.dashboardViewMode) {
            this.reloadFullAsyncData();
        }
    },
    updated: function () {
        SidebarService.activateSidebarRoute();

        if (this.dashboardViewMode) {
            this.reloadThemeColor();
        }
    },
    beforeDestroy: function () {
        SocketManager.closeAllConnections();
    },
    components: {
        MenuComponent: require('../navmenu/navmenu.vue.html')
    },
    watch: {
        '$route': function (to) {
            this.switchViewMode(to.matched);
        }
    },
    computed: {
        passProps: function () {            
            return {
                account: this.account,
                activeGroup: this.activeGroup,
                users: this.users,
                usersOutsidePrivateChats: this.usersOutsidePrivateChats,
                channels: this.channels,
                connectedChannels: this.connectedChannels,
                connectedChats: this.connectedChats,
                chatsCount: this.chatsCount,
                roleInActiveGroup: this.roleInActiveGroup
            }
        }
    },
    methods: {
        switchViewMode: function(matched) {
            let oldViewMode = this.dashboardViewMode;
            this.dashboardViewMode = !matched.some(record => record.meta.anonymous);
            let viewModeChanged = oldViewMode !== this.dashboardViewMode;
            
            if (viewModeChanged) {
                this.adaptToViewMode();
            }
            
            if (this.dashboardViewMode) {
                this.reloadThemeColor();
            }
        },
        adaptToViewMode: function () {
            if (this.dashboardViewMode) {
                ThemeService.disableLoginTheme();
                NotificationService.displayAllForAuthorized();
            } else {
                ThemeService.enableLoginTheme();
                NotificationService.displayAllForAnonymous();
            }
        },
        reloadFullAsyncData: function (mode = 'normal') {
            if (AuthenticationService.isAuthenticated()) {
                let loader = this.$loading.show();

                Promise.all([
                    this.getAuthorizedAccountPromise(),
                    this.getGroupsPromise(),
                    this.getEventsPromise()
                ]).then(
                    ([accountResponse, groupsResponse, eventsResponse]) => {
                        this.account = accountResponse.body;
                        this.groups = groupsResponse.body;
                        this.events = eventsResponse.body;

                        loader.hide();
                        this.redirectWhenUserHaveNoGroups();

                        this.computeActiveGroup();
                        this.reloadAsyncData(mode);
                        this.openAndEmitEventForSockets();
                    },
                    (error) => {
                        loader.hide();
                        this.handleFullAsyncHttpError(error);
                    }
                );
            }
        },
        reloadAsyncData: function(mode = 'normal') {
            if (AuthenticationService.isAuthenticated()) {
                this.redirectWhenUserHaveNoGroups();
                
                if (TypeCheckService.isEmptyObject(this.activeGroup)) { 
                    return; 
                }
    
                let loader = this.$loading.show();
                
                Promise.all([
                    this.getRoleInActiveGroupPromise(),
                    this.getUsersPromise(),
                    this.getUsersOutsidePrivateChatsPromise(),
                    this.getChannelsPromise(),
                    this.getConnectedChannelsPromise(),
                    this.getConnectedChatsPromise(),
                    this.getChatsCountPromise()
                ]).then(
                    ([
                        roleResponse,
                        usersResponse, 
                        usersOutsidePrivateChatsResponse, 
                        channelsResponse, 
                        connectedChannelsResponse, 
                        connectedChatsResponse, 
                        chatsCountResponse, 
                     ]) => {
                        this.roleInActiveGroup = roleResponse.body;
                        this.users = usersResponse.body;
                        this.usersOutsidePrivateChats = usersOutsidePrivateChatsResponse.body;
                        this.channels = channelsResponse.body;
                        this.connectedChannels = connectedChannelsResponse.body;
                        this.connectedChats = connectedChatsResponse.body;
                        this.chatsCount = chatsCountResponse.body;
    
                        loader.hide();
                        NotificationService.displayAllForAuthorized();
                        this.$events.emit("AsyncDataReloaded");
                    },
                    (error) => {
                        loader.hide();
                        this.handleAsyncHttpError(error, mode);
                    }
                );
            }
        },
        retryConnectionAfterTimeout: function (secondsTimeout = 5) {
            if (AuthenticationService.isAuthenticated()) {
                DialogService.showConnectionProblemDialogWithTimeout(secondsTimeout);
                
                setTimeout(
                    () => this.reloadFullAsyncData(),
                    secondsTimeout * 1000
                );
            }
        },
        redirectWhenUserHaveNoGroups: function () {
            let routeNotAllowed = !this.$route.matched.some(record => record.meta.allowedWithoutGroups);
            
            if (this.userHaveNotGroups() && routeNotAllowed) {
                this.$router.push('/group/create');
            }
        }
    }
}
