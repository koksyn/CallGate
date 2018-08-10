import {EventTypes} from "../stores/EventStore";
import NotificationService from "../services/ui/NotificationService";

export default {
    mounted: function () {
        this.$events.on(
            'SocketEventType:' + EventTypes.UserAddedToGroup, 
            (event) => {
                (this.isLoggedAccountId(event.userId))
                    ? this.handleAuthorizedUserAddedToGroup(event)
                    : this.handleOtherUserAddedToGroup(event);
            }
        );
    },
    methods: {
        handleAuthorizedUserAddedToGroup: function (event) {
            let message = `You have been successfully added to group <b>${event.groupName}</b>`;

            if (!this.isLoggedAccountId(event.authorUserId)) {
                message += ` by user <b>${event.authorUsername}</b>`;
            }
            
            NotificationService.displayNotification(message);
            this.reloadFullAsyncData();
        },
        handleOtherUserAddedToGroup: function (event) {            
            if (this.activeGroup.id === event.groupId) {
                let message = `User <b>${event.username}</b> has been added to group <b>${event.groupName}</b>`;
                let color = 'alert-success';

                if (!this.isLoggedAccountId(event.authorUserId)) {
                    message += ` by user <b>${event.authorUsername}</b>`;
                    color = 'bg-blue-grey';
                }
                
                NotificationService.displayNotification(message, color);
                this.reloadAsyncData();
            }
        }
    }
}