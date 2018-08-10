import {EventTypes} from "../stores/EventStore";
import NotificationService from "../services/ui/NotificationService";

export default {
    mounted: function () {
        this.$events.on(
            'SocketEventType:' + EventTypes.UserRemovedFromGroup, 
            (event) => {
                (this.isLoggedAccountId(event.userId))
                    ? this.handleAuthorizedUserRemovedFromGroup(event)
                    : this.handleOtherUserRemovedFromGroup(event);
            }
        );
    },
    methods: {
        handleAuthorizedUserRemovedFromGroup: function (event) {
            let message = (this.isLoggedAccountId(event.authorUserId)) 
                ? 'You have been successfully removed'
                : `User <b>${event.authorUsername}</b> removed you`;
            
            message += ` from the group <b>${event.groupName}</b>.`;
            
            NotificationService.displayNotification(message, 'bg-deep-purple');
            this.reloadFullAsyncData();
            
            if (this.activeGroup.id === event.groupId) {
                this.$router.push('/');
            }
        },
        handleOtherUserRemovedFromGroup: function (event) {
            if (this.activeGroup.id === event.groupId) {
                let message = `User <b>${event.username}</b> has been removed from group <b>${event.groupName}</b>`;
                let color = 'bg-deep-purple';
                
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