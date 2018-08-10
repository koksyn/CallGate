import Vue from "vue";
import AuthenticationService from "../../services/AuthenticationService";
import VueResource from "vue-resource";
import VueRouter from 'vue-router';
import CacheService from "../../services/CacheService";
import BasicValidationService from "../../services/utils/BasicValidationService";
import HttpErrorHandler from "../../mixins/HttpErrorHandler";

Vue.use(VueResource);
Vue.use(VueRouter);

export default {
    mixins: [HttpErrorHandler],
    data: function () {
        return {
            username: null,
            password: null,
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
         login: function(event) {
            event.preventDefault();
            AuthenticationService.clearAuthenticationToken();
            this.error = false;
            
            let formData = new FormData();
            formData.append("username", this.username);
            formData.append("password", this.password);

            this.$http.post("/authentication/login", formData).then(
                (response) => {
                    AuthenticationService.setAuthenticationToken(response.body.token, new Date(response.body.expirationDate));
                    AuthenticationService.setBearerToken();

                    if (AuthenticationService.isAuthenticated()) {
                        CacheService.clear();
                        this.$parent.reloadFullAsyncData();
                        this.$router.push("/");
                    }
                },
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