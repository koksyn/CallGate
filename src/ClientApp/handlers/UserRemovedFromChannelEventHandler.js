import {EventTypes} from "../stores/EventStore";
import NotificationService from "../services/ui/NotificationService";

export default {
    mounted: function () {
        this.$events.on(
            'SocketEventType:' + EventTypes.UserRemovedFromChannel, 
            (event) => {
                (this.isLoggedAccountId(event.userId))
                    ? this.handleAuthorizedUserRemovedFromChannel(event)
                    : this.handleOtherUserRemovedFromChannel(event);
            }
        );
    },
    methods: {
        handleAuthorizedUserRemovedFromChannel: function (event) {
            let message = `You have been removed from channel <b>${event.channelName}</b>`;

            if(!this.isLoggedAccountId(event.authorUserId)) {
                message += ` by user <b>${event.authorUsername}</b>`;
            }
            
            if (this.activeGroup.id === event.groupId) {
                NotificationService.displayNotification(message, 'bg-deep-purple');
                this.$events.once('AsyncDataReloaded', () => this.redirectToHomeWhenChannelOpened(event.channelId));
                this.reloadAsyncData();
            } else {
                message += ` in group <b>${event.groupName}</b>`;
                
                NotificationService.displayNotification(message, 'bg-deep-purple');
            }
        },
        handleOtherUserRemovedFromChannel: function (event) {
            let displayMessage = false;
            let message = `User <b>${event.username}</b> has been removed from channel <b>${event.channelName}</b>`;

            if (this.isChannelViewOpened(event.channelId) && !this.isLoggedAccountId(event.authorUserId)) {
                message += ` by user <b>${event.authorUsername}</b>`;

                if (this.activeGroup.id !== event.groupId) {
                    message += ` in group <b>${event.groupName}</b>`;
                }
                
                displayMessage = true;
            }

            if (displayMessage || this.isLoggedAccountId(event.authorUserId)) {
                NotificationService.displayNotification(message, 'bg-deep-purple');
            }
        },
        redirectToHomeWhenChannelOpened: function (channelId) {
            if (this.isChannelViewOpened(channelId)) {
                this.$router.push('/');
            }
        }
    }
}