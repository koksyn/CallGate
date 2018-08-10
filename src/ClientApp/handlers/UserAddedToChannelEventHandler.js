import {EventTypes} from "../stores/EventStore";
import NotificationService from "../services/ui/NotificationService";

export default {
    mounted: function () {
        this.$events.on(
            'SocketEventType:' + EventTypes.UserAddedToChannel, 
            (event) => {
                (this.isLoggedAccountId(event.userId))
                    ? this.handleAuthorizedUserAddedToChannel(event)
                    : this.handleOtherUserAddedToChannel(event);
            }
        );
    },
    methods: {
        handleAuthorizedUserAddedToChannel: function (event) {
            let message = `You have been successfully added to channel <b>${event.channelName}</b>`;

            if(!this.isLoggedAccountId(event.authorUserId)) {
                message += ` by user <b>${event.authorUsername}</b>`;
            }

            if (this.activeGroup.id === event.groupId) {
                NotificationService.displayNotification(message);

                if(this.isLoggedAccountId(event.authorUserId)) {
                    this.$events.once('AsyncDataReloaded', () => this.redirectToChannel(event.channelId));
                }
                
                this.reloadAsyncData();
            } else {
                message += ` in group <b>${event.groupName}</b>`;
                
                NotificationService.displayNotification(message);
            }
        },
        handleOtherUserAddedToChannel: function (event) {
            let displayMessage = false;
            let message = `User <b>${event.username}</b> has been added to channel <b>${event.channelName}</b>`;
            
            if (this.isChannelViewOpened(event.channelId) && !this.isLoggedAccountId(event.authorUserId)) {
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
        },
        redirectToChannel: function (channelId) {
            this.$router.push(`/channel/${channelId}`);
        },
        isChannelViewOpened: function (channelId) {
            return this.$route.fullPath.startsWith(`/channel/${channelId}`);
        }
    }
}