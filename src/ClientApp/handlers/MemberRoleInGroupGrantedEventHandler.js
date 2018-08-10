import {EventTypes} from "../stores/EventStore";
import NotificationService from "../services/ui/NotificationService";

export default {
    mounted: function () {
        this.$events.on(
            'SocketEventType:' + EventTypes.MemberRoleInGroupGranted, 
            (event) => {
                (this.isLoggedAccountId(event.userId))
                    ? this.handleAuthorizedUserMemberRoleInGroupGranted(event)
                    : this.handleOtherUserMemberRoleInGroupGranted(event);
            }
        );
    },
    methods: {
        handleAuthorizedUserMemberRoleInGroupGranted: function (event) {
            let message = '<b>Member</b> role has been granted to your account.';
            message += ` Action made by administrator: <b>${event.authorUsername}</b>`;
            
            if (event.groupId === this.activeGroup.id) {
                NotificationService.displayNotification(message);
                this.reloadFullAsyncData();

                if (this.isRouteForGroupAdminOpened()) {
                    this.$router.push('/');
                }
            } else {
                message += ` in group <b>${event.groupName}</b>`;
                
                NotificationService.displayNotification(message);
            }
        },
        handleOtherUserMemberRoleInGroupGranted: function (event) {
            if (event.groupId === this.activeGroup.id) {
                if (this.isLoggedAccountId(event.authorUserId) || this.isGroupUserManageViewOpened()) {
                    let message = `<b>Member</b> role for user <b>${event.username}</b>`;
                    message += ` in this group has been granted.`;

                    let color = 'alert-success';

                    if (!this.isLoggedAccountId(event.authorUserId)) {
                        message += ` Action made by administrator: <b>${event.authorUsername}</b>`;
                        color = 'bg-blue-grey';
                    }

                    NotificationService.displayNotification(message, color);
                }
            }
        },
        isRouteForGroupAdminOpened: function () {
            return this.$route.matched.some(record => record.meta.onlyForGroupAdmin);
        },
        isGroupUserManageViewOpened: function () {
            return this.$route.fullPath.startsWith('/group/user/manage');
        }
    }
}