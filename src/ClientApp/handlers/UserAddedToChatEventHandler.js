import {EventTypes} from "../stores/EventStore";
import NotificationService from "../services/ui/NotificationService";

export default {
    mounted: function () {
        this.$events.on(
            'SocketEventType:' + EventTypes.UserAddedToChat, 
            (event) => {
                (this.isLoggedAccountId(event.userId))
                    ? this.handleAuthorizedUserAddedToChat(event)
                    : this.handleOtherUserAddedToChat(event);
            }
        );
    },
    methods: {
        handleAuthorizedUserAddedToChat: function (event) {
            let message = 'You have been successfully added to chat';

            if(!this.isLoggedAccountId(event.authorUserId)) {
                message += ` by user <b>${event.authorUsername}</b>`;
            }

            if (this.activeGroup.id === event.groupId) {
                NotificationService.displayNotification(message);

                if(this.isLoggedAccountId(event.authorUserId)) {
                    this.$events.once('AsyncDataReloaded', () => this.redirectToChat(event.chatId));
                }

                this.reloadAsyncData();
            } else {
                message += ` in group <b>${event.groupName}</b>`;

                NotificationService.displayNotification(message);
            }
        },
        handleOtherUserAddedToChat: function (event) {
            let displayMessage = false;
            let message = `User <b>${event.username}</b> has been added to this chat`;

            if (this.isChatViewOpened(event.chatId) && !this.isLoggedAccountId(event.authorUserId)) {
                message += ` by user <b>${event.authorUsername}</b>`;
                
                if (this.activeGroup.id !== event.groupId) {
                    message += ` in group <b>${event.groupName}</b>`;
                }
                
                displayMessage = true;
            }

            if (displayMessage || this.isLoggedAccountId(event.authorUserId)) {
                let color = displayMessage ? 'bg-blue-grey' : 'alert-success';

                NotificationService.displayNotification(message, color);
            }
            
            this.reloadAsyncData();
        },
        redirectToChat: function (chatId) {
            this.$router.push(`/chat/${chatId}`);
        },
        isChatViewOpened: function (chatId) {
            return this.$route.fullPath.startsWith(`/chat/${chatId}`);
        }
    }
}