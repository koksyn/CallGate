import ModalService from "../../services/ui/ModalService";
import DateTimeFormatter from "../../mixins/DatetimeFormatter";
import RoleChecker from "../../mixins/RoleChecker";
import HttpErrorHandler from "../../mixins/HttpErrorHandler";

export default {
    mixins: [DateTimeFormatter, RoleChecker, HttpErrorHandler],
    props: [
        'users', 
        'channels', 
        'connectedChannels', 
        'activeGroup', 
        'connectedChats', 
        'chatsCount'
    ],
    methods: {
        leaveGroupAttempt: function (event) {
            event.preventDefault();
            ModalService.openModal('leaveGroup', 'deep-purple');
        },
        leaveGroup: function() {
            this.$http.delete("/api/group/" + this.activeGroup.id + "/user/authorized").catch(
                (error) => this.handleHttpNotValidationErrors(error)
            );
        }
    }
}