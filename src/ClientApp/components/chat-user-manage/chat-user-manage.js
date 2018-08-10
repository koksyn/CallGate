import DateTimeFormatter from "../../mixins/DatetimeFormatter";
import RoleChecker from "../../mixins/RoleChecker";
import {EventTypes} from "../../stores/EventStore";
import DetailsPreloader from "../../mixins/DetailsPreloader";
import HttpErrorHandler from "../../mixins/HttpErrorHandler";

export default {
    mixins: [DateTimeFormatter, RoleChecker, DetailsPreloader, HttpErrorHandler],
    props: ["account"],
    data: function () {
        return {
            chatId: "",
            chatUsers: [],
            groupUsersOutsideChat: [],
        };
    },
    mounted: function () {
        this.reloadContext();
        
        this.$events.on(`SocketEventType:${EventTypes.UserAddedToChat}`,(event) => {
            this.reloadContext(event.chatId);
        });
        this.$events.on(`SocketEventType:${EventTypes.UserRemovedFromChat}`,(event) => {
            this.reloadContext(event.chatId);
        });
        this.$events.on(`SocketEventType:${EventTypes.UserAddedToGroup}`,(event) => {
            if (event.groupId === this.$parent.activeGroup.id) {
                this.reloadContext();
            }
        });
        this.$events.on(`SocketEventType:${EventTypes.UserRemovedFromGroup}`,(event) => {
            if (event.groupId === this.$parent.activeGroup.id) {
                this.reloadContext();
            }
        });
    },
    watch: {
        '$route.params.id': function () {
            this.reloadContext();
        }
    },
    methods: {
        reloadContext: function (chatId) {
            this.parseRouteParams();
            
            if ((chatId !== undefined) && (chatId !== this.chatId)) {
                return;
            }

            this.showDetailsPreloader();

            Promise.all([
                this.getChatUsersPromise(),
                this.getGroupUsersOutsideChatPromise()
            ]).then(([
                chatUsersResponse,
                groupUsersOutsideChatResponse
            ]) => {
                this.chatUsers = chatUsersResponse.body;
                this.groupUsersOutsideChat = groupUsersOutsideChatResponse.body;
                this.hideDetailsPreloader();
            },
            (error) => {
                this.hideDetailsPreloader();
                this.handleHttpNotValidationErrors(error);
            });
        },
        parseRouteParams: function () {
            this.chatId = this.$route.params.id;
        },
        getChatUsersPromise: function () {
            return this.$http.get("/api/chat/" + this.chatId + "/user");
        },
        getGroupUsersOutsideChatPromise: function () {
            return this.$http.get("/api/chat/" + this.chatId + "/user/groupUsersOutsideChat");
        },
        addUserToChat: function (user) {
            let formData = new FormData();
            formData.append("Username", user.username);

            this.$http.post("/api/chat/" + this.chatId + "/user", formData).catch(
                (error) => this.handleHttpNotValidationErrors(error)
            );
        }
    }
}