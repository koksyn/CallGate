import {EventTypes} from "../stores/EventStore";
import NotificationService from "../services/ui/NotificationService";

export default {
    mounted: function () {        
        this.$events.on(
            'SocketEventType:' + EventTypes.ChannelCreated,
            (event) => {                
                (this.isLoggedAccountId(event.authorUserId))
                    ? this.handleChannelCreatedByAuthorizedUser(event)
                    : this.handleChannelCreatedByOtherUser(event);
            }
        );
    },
    methods: {
        handleChannelCreatedByAuthorizedUser: function (event) {
            let message = `New channel <b>${event.channelName}</b> was sucessfully created.`;
            NotificationService.displayNotification(message);
            
            this.$events.once('AsyncDataReloaded', () => this.redirectToCreatedChannel(event));
            this.reloadAsyncData();
        },
        handleChannelCreatedByOtherUser: function (event) {
            if (this.activeGroup.id === event.groupId) {
                let message = `User <b>${event.authorUsername}</b> has created a new channel <b>${event.channelName}</b>.`;
                NotificationService.displayNotification(message, 'bg-blue-grey');
                
                this.reloadAsyncData();
            }
        },
        redirectToCreatedChannel: function (event) {
            this.$router.push('/channel/' + event.channelId);
        }
    }
}