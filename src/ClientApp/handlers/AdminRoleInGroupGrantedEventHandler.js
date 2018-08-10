import {EventTypes} from "../stores/EventStore";
import NotificationService from "../services/ui/NotificationService";

export default {
    mounted: function () {
        this.$events.on(
            'SocketEventType:' + EventTypes.AdminRoleInGroupGranted, 
            (event) => {
                (this.isLoggedAccountId(event.userId))
                    ? this.handleAuthorizedUserAdminRoleInGroupGranted(event)
                    : this.handleOtherUserAdminRoleInGroupGranted(event);
            }
        );
    },
    methods: {
        handleAuthorizedUserAdminRoleInGroupGranted: function (event) {
            let message = '<b>Admin</b> role has been granted to your account.';
            message += ` Action made by administrator: <b>${event.authorUsername}</b>`;

            if (event.groupId === this.activeGroup.id) {
                this.reloadFullAsyncData();
            } else {
                message += ` in group <b>${event.groupName}</b>`;
            }
            
            NotificationService.displayNotification(message);
        },
        handleOtherUserAdminRoleInGroupGranted: function (event) {
            if (event.groupId === this.activeGroup.id) {
                if (this.isLoggedAccountId(event.authorUserId) || this.isGroupUserManageViewOpened()) {
                    let message = `<b>Admin</b> role for user <b>${event.username}</b>`;
                    message += ` in this group has been granted.`;

                    let color = 'alert-success';

                    if (!this.isLoggedAccountId(event.authorUserId)) {
                        message += ` Action made by administrator: <b>${event.authorUsername}</b>`;
                        color = 'bg-blue-grey';
                    }
                    
                    NotificationService.displayNotification(message, color);
                }
            }
        }
    }
}