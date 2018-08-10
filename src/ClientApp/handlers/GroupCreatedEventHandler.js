import {EventTypes} from "../stores/EventStore";
import NotificationService from "../services/ui/NotificationService";

export default {
    mounted: function () {
        this.$events.on(
            'SocketEventType:' + EventTypes.GroupCreated, 
            (event) => { this.handleGroupCreatedByAuthorizedUser(event) }
        );
    },
    methods: {
        handleGroupCreatedByAuthorizedUser: function (event) {
            let message = 'New group called <b>' + event.groupName + '</b> was sucessfully created.';
            NotificationService.displayNotification(message);

            this.$events.once('AsyncDataReloaded', () => this.activateCreatedGroup(event));
            this.reloadFullAsyncData();
        },
        activateCreatedGroup: function (event) {
            this.activateGroupById(event.groupId);
        }
    }
}