import {EventTypes} from "../stores/EventStore";
import NotificationService from "../services/ui/NotificationService";

export default {
    mounted: function () {
        this.$events.on(
            'SocketEventType:' + EventTypes.UserRemovedFromChat, 
            (event) => {
                (this.isLoggedAccountId(event.userId))
                    ? this.handleAuthorizedUserRemovedFromChat(event)
                    : this.handleOtherUserRemovedFromChat(event);
            }
        );
    },
    methods: {
        handleAuthorizedUserRemovedFromChat: function (event) {
            let message = 'You have been successfully removed from chat';

            if(!this.isLoggedAccountId(event.authorUserId)) {
                message += ` by user <b>${event.authorUsername}</b>`;
            }                    
            
            if (this.activeGroup.id === event.groupId) {
                NotificationService.displayNotification(message, 'bg-deep-purple');
                this.$events.once('AsyncDataReloaded', () => this.redirectToHomeWhenChatOpened(event.chatId));
                this.reloadAsyncData();
            } else {
                message += ` in group <b>${event.groupName}</b>`;
                
                NotificationService.displayNotification(message, 'bg-deep-purple');
            }
        },
        handleOtherUserRemovedFromChat: function (event) {
            let displayMessage = false;
            let message = `User <b>${event.username}</b> has been removed from chat`;

            if (this.isChatViewOpened(event.chatId) && !this.isLoggedAccountId(event.authorUserId)) {
                message += ` by user <b>${event.authorUsername}</b>`;

                if (this.activeGroup.id !== event.groupId) {
                    message += ` in group <b>${event.groupName}</b>`;
                }
               
                displayMessage = true;
            }

            if (displayMessage || this.isLoggedAccountId(event.authorUserId)) {
                NotificationService.displayNotification(message, 'bg-deep-purple');
            }
            
            this.reloadAsyncData();
        },
        redirectToHomeWhenChatOpened: function (chatId) {
            if (this.isChatViewOpened(chatId)) {
                this.$router.push('/');
            }
        }
    }
}