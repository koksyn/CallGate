import {EventTypes} from "../stores/EventStore";
import NotificationService from "../services/ui/NotificationService";

export default {
    mounted: function () {
        this.$events.on(
            'SocketEventType:' + EventTypes.ChatCreated,
            (event) => {
                (this.isLoggedAccountId(event.authorUserId))
                    ? this.handleChatCreatedByAuthorizedUser(event)
                    : this.handleChatCreatedByOtherUser(event);
            }
        );
    },
    methods: {
        handleChatCreatedByAuthorizedUser: function (event) {
            let message = 'New chat was sucessfully created.';
            NotificationService.displayNotification(message);

            this.$events.once('AsyncDataReloaded', () => this.redirectToCreatedChat(event));
            this.reloadAsyncData();
        },
        handleChatCreatedByOtherUser: function (event) {
            if (this.activeGroup.id === event.groupId) {
                let message = `User <b>${event.authorUsername}</b> has created a new chat with you.`;
                NotificationService.displayNotification(message, 'bg-blue-grey');

                this.reloadAsyncData();
            }
        },
        redirectToCreatedChat: function (event) {
            this.$router.push('/chat/' + event.chatId);
        }
    }
}