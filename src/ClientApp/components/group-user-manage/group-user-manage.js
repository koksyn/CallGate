import DateTimeFormatter from "../../mixins/DatetimeFormatter";
import RoleChecker from "../../mixins/RoleChecker";
import ModalService from "../../services/ui/ModalService";
import SelectPickerService from "../../services/ui/SelectPickerService";
import {EventTypes} from "../../stores/EventStore";
import DetailsPreloader from "../../mixins/DetailsPreloader";
import HttpErrorHandler from "../../mixins/HttpErrorHandler";

export default {
    mixins: [DateTimeFormatter, RoleChecker, DetailsPreloader, HttpErrorHandler],
    props: ["account", "activeGroup"],
    data: function () {
        return {
            detailedGroupUsers: [],
            choosedUser: null
        };
    },
    mounted: function () {
        if (this.$parent.isActiveGroupNotEmpty()) {
            this.reloadDetailedGroupUsers();
        }
        
        this.$events.on('AsyncDataReloaded', () => this.reloadDetailedGroupUsers());
        
        this.$events.on(`SocketEventType:${EventTypes.MemberRoleInGroupGranted}`, (event) => {
            if (event.groupId === this.activeGroup.id) {
                this.reloadDetailedGroupUsers();
            }
        });
        this.$events.on(`SocketEventType:${EventTypes.AdminRoleInGroupGranted}`, (event) => {
            if (event.groupId === this.activeGroup.id) {
                this.reloadDetailedGroupUsers();
            }
        });
    },
    updated: function () {
        SelectPickerService.disposeAllPickers();
        SelectPickerService.invokeAllPickers();
    },
    methods: {
        editUserRole: function (user) {
            let formData = new FormData();
            formData.append("Username", user.username);
            formData.append("Role", user.role);

            this.$http.put("/api/group/" + this.$parent.activeGroup.id + "/user", formData).catch(
                (error) => this.handleHttpNotValidationErrors(error)
            );
        },
        removeUserFromGroupAttempt: function (user, event) {
            if (event) event.preventDefault();
            
            this.choosedUser = user;
            ModalService.openModal('removeUser', 'deep-purple');
        },
        removeUserFromGroup: function () {
            if (this.choosedUser !== null) {
                let user = this.choosedUser;
                
                this.$http.delete("/api/group/" + this.$parent.activeGroup.id + "/user/?username=" + user.username).catch(
                    (error) => this.handleHttpNotValidationErrors(error)
                );
            }
        },
        reloadDetailedGroupUsers: function () {
            this.showDetailsPreloader();
            
            this.$http.get("/api/group/" + this.$parent.activeGroup.id + "/user/details").then(
                (response) => {
                    SelectPickerService.disposeAllPickers();
                    this.detailedGroupUsers = response.body;
                    this.hideDetailsPreloader();
                },
                (error) => {
                    this.hideDetailsPreloader();
                    this.handleHttpNotValidationErrors(error);
                }
            );
        }
    }
}