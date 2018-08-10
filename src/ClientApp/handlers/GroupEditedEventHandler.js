import {EventTypes} from "../stores/EventStore";
import NotificationService from "../services/ui/NotificationService";

export default {
    mounted: function () {
        this.$events.on(
            'SocketEventType:' + EventTypes.GroupEdited, 
            (event) => { 
                (this.isLoggedAccountId(event.authorUserId))
                    ? this.handleGroupEditedByAuthorizedUser(event)
                    : this.handleGroupEditedByOtherUser(event);
            }
        );
    },
    methods: {
        handleGroupEditedByAuthorizedUser: function (event) {
            let message = 'Group details were sucessfully updated.';
            NotificationService.displayNotification(message);

            this.reloadFullAsyncData();
        },
        handleGroupEditedByOtherUser: function (event) {
            let message = `Group <b>${event.groupName}</b> details were updated by user <b>${event.authorUsername}</b>`;
            NotificationService.displayNotification(message, 'bg-blue-grey');
            
            this.reloadFullAsyncData();
        }
    }
}