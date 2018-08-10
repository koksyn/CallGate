import Vue from "vue";

import VueResource from "vue-resource";
import VueRouter from 'vue-router';
import BasicValidationService from "../../services/utils/BasicValidationService";
import HttpErrorHandler from "../../mixins/HttpErrorHandler";

Vue.use(VueResource);
Vue.use(VueRouter);

export default {
    mixins: [HttpErrorHandler],
    data: function() {
        return {
            succeded: false,
            errors: []
        }
    },
    mounted: function () {
        this.register();
    },
    methods: {
        register: function() {
            let formData = new FormData();
            formData.append("confirmationPhrase", this.$route.query.confirmationPhrase);
  
            this.$http.patch("/authentication/confirmRegistration", formData).then(
                (response) => { this.succeded = true; },
                (error) => {
                    console.log(error);
                    
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