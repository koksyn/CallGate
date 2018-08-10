import BasicValidationService from "../../services/utils/BasicValidationService";
import HttpErrorHandler from "../../mixins/HttpErrorHandler";

export default {
    mixins: [HttpErrorHandler],
    data: function () {
        return {
            name: "",
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
        createChannel: function () {
            this.errors = [];

            let formData = new FormData();
            formData.append("Name", this.name);
            formData.append("GroupId", this.$parent.activeGroup.id);
            
            this.$http.post("/api/channel/", formData).catch(
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