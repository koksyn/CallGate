import Vue from "vue";
import VueResource from "vue-resource";
import BasicValidationService from "../../services/utils/BasicValidationService";
import ModalService from "../../services/ui/ModalService";
import HttpErrorHandler from "../../mixins/HttpErrorHandler";

Vue.use(VueResource);

export default {
    mixins: [HttpErrorHandler],
    data: function() {
        return {
            username: null,
            email: null,
            password: null,
            confirm: null,
            errors: []
        }
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
        register: function(event) {
            event.preventDefault();
            this.errors = [];
            
            if (this.password !== this.confirm) {
                this.errors.push("Passwords are not identical!");
                return;
            }
            
            let formData = new FormData();
            formData.append("username", this.username);
            formData.append("email", this.email);
            formData.append("password", this.password);
            formData.append("redirectUrl", location.protocol + "//" + location.host + "/confirmRegistration");
        
            this.$http.post("/authentication/register", formData).then(
                (response) => ModalService.openModal('successRegistration'),
                (error) => {
                    console.log(error);

                    if (this.isValidationError(error)) {
                        this.errors = BasicValidationService.getArrayOfErrorsFromResponse(error);
                    } else {
                        this.handleHttpNotValidationErrors(error);
                    }
                }
            );
        },
    }
}