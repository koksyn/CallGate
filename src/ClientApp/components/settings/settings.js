import ModalService from "../../services/ui/ModalService";
import NotificationService from "../../services/ui/NotificationService";
import BasicValidationService from "../../services/utils/BasicValidationService";
import HttpErrorHandler from "../../mixins/HttpErrorHandler";
import Logout from "../../mixins/Logout";

export default {
    mixins: [HttpErrorHandler, Logout],
    data: function () {
        return {
            password: "",
            errors: []
        };
    },
    computed: {
        classWhenError: function () {
            return {
                error: this.errors.length > 0,
                focused: this.errors.length > 0,
            }
        }
    },
    methods: {
        deleteAccount: function (event) {
            ModalService.openModal('deleteAccount', 'deep-purple');
        },
        confirmDeleteAccount: function (event) {
            let formData = {
                params: {
                    password: this.password
                }
            };

            this.errors = [];
            
            this.$http.delete("/api/account/authorized", formData).then(
                (response) => {
                    NotificationService.clearStack();
                    NotificationService.pushAnonymous('Your account has been successfully deleted - on your demand.');
                    
                    this.logout();
                },
                (error) => { 
                    if (this.isValidationError(error)) {
                        this.errors = BasicValidationService.getArrayOfErrorsFromResponse(error);
                    } else {
                        this.handleHttpNotValidationErrors(error);
                    }
                }
            );
        }
    }
}