import DateTimeFormatter from "../../mixins/DatetimeFormatter";
import RoleChecker from "../../mixins/RoleChecker";
import ChannelLeave from "../../mixins/ChannelLeave";
import {EventTypes} from "../../stores/EventStore";
import DetailsPreloader from "../../mixins/DetailsPreloader";
import HttpErrorHandler from "../../mixins/HttpErrorHandler";

export default {
    mixins: [DateTimeFormatter, RoleChecker, ChannelLeave, DetailsPreloader, HttpErrorHandler],
    props: ["account"],
    data: function () {
        return {
            channelId: "",
            channel: {},
            channelUsers: [],
            groupUsersOutsideChannel: []
        };
    },
    mounted: function () {
        this.reloadContext();
        
        this.$events.on(
            `SocketEventType:${EventTypes.UserAddedToChannel}`, 
            (event) => this.reloadChannelWithUsers(event.channelId)
        );
        this.$events.on(
            `SocketEventType:${EventTypes.UserRemovedFromChannel}`,
            (event) => this.reloadChannelWithUsers(event.channelId)
        );
        this.$events.on(`SocketEventType:${EventTypes.UserAddedToGroup}`,(event) => {
            if (event.groupId === this.$parent.activeGroup.id) {
                this.reloadChannelWithUsers();
            }
        });
        this.$events.on(`SocketEventType:${EventTypes.UserRemovedFromGroup}`,(event) => {
            if (event.groupId === this.$parent.activeGroup.id) {
                this.reloadChannelWithUsers();
            }
        });
    },
    watch: {
        '$route.params.id': function () {
            this.reloadContext();
        }
    },
    methods: {
        reloadContext: function () {
            this.parseRouteParams();
            this.reloadChannelWithUsers();
        },
        parseRouteParams: function () {
            this.channelId = this.$route.params.id;
        },
        reloadChannelWithUsers: function (channelId) {
            if ((channelId !== undefined) && (channelId !== this.channelId)) {
                return;
            }
            
            this.showDetailsPreloader();
            
            this.$http.get("/api/channel/" + this.channelId).then(
                (channelResponse) => {
                    this.channel = channelResponse.body;
                    this.reloadChannelRelatedUsers();
                },
                (error) => {
                    this.hideDetailsPreloader();
                    this.handleHttpNotValidationErrors(error);
                }
            );
        },
        reloadChannelRelatedUsers: function () {
            Promise.all([
                this.getUsersInsideChannelPromise(),
                this.getGroupUsersOutsideChannelPromise()
            ]).then(([
                usersInsideChannelResponse,
                groupUsersOutsideChannelResponse
            ]) => {
                this.channelUsers = usersInsideChannelResponse.body;
                this.groupUsersOutsideChannel = groupUsersOutsideChannelResponse.body;
                this.hideDetailsPreloader();
            },
            (error) => {
                this.hideDetailsPreloader();
                this.handleHttpNotValidationErrors(error);
            });    
        },
        getUsersInsideChannelPromise: function () {
            return this.$http.get("/api/channel/" + this.channelId + "/user");
        },
        getGroupUsersOutsideChannelPromise: function () {
            return this.$http.get("/api/channel/" + this.channelId + "/user/groupUsersOutsideChannel");
        },
        addUserToChannel: function (user) {
            let formData = new FormData();
            formData.append("Username", user.username);
            
            this.$http.post("/api/channel/" + this.channelId + "/user", formData).catch(
                (error) => this.handleHttpNotValidationErrors(error)
            );
        },
        removeUserFromChannel: function (user) {            
            this.$http.delete("/api/channel/" + this.channelId + "/user?username=" + user.username).catch(
                (error) => this.handleHttpNotValidationErrors(error)
            );
        }
    }
}