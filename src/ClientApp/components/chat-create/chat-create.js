import BasicValidationService from "../../services/utils/BasicValidationService";
import {EventTypes} from "../../stores/EventStore";
import HttpErrorHandler from "../../mixins/HttpErrorHandler";

export default {
    mixins: [HttpErrorHandler],
    data: function () {
        return {
            user: null,
            errors: []
        }
    },
    mounted: function () {
        this.reloadUserOutsidePrivateChats();
        
        let routeUserId = this.$route.params.userId;
        let isLoggedAccountId = (userId) => this.$parent.isLoggedAccountId(userId);
        
        this.$events.on(`SocketEventType:${EventTypes.UserAddedToChat}`, (event) => {
            if ((event.authorUserId === routeUserId) && (isLoggedAccountId(event.userId))) {
                this.$router.push(`/chat/${event.chatId}`);
            }
        });
    },
    watch: {
        '$route.params.userId': function () {
            this.reloadUserOutsidePrivateChats();
        },
        '$parent.usersOutsidePrivateChats': function () {
            this.reloadUserOutsidePrivateChats();
        }
    },
    methods: {
        createChat: function () {
            this.errors = [];
            
            let formData = new FormData();
            formData.append("Username", this.user.username);
            formData.append("GroupId", this.$parent.activeGroup.id);

            this.$http.post("/api/chat/", formData).catch(
                (error) => {
                    if (this.isValidationError(error)) {
                        this.errors = BasicValidationService.getArrayOfErrorsFromResponse(error);
                    } else {
                        this.handleHttpNotValidationErrors(error);
                    }
                }
            );
        },
        reloadUserOutsidePrivateChats() {
            let userId = this.$route.params.userId;
            this.user = this.$parent.getUserByIdOutsidePrivateChats(userId);
        },
    }
}