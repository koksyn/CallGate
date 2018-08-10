import BasicValidationService from "../../services/utils/BasicValidationService";
import HttpErrorHandler from "../../mixins/HttpErrorHandler";

export default {
    mixins: [HttpErrorHandler],
    data: function () {
        return {
            name: "",
            description: "",
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
        createGroup: function () {
            this.errors = [];

            let formData = new FormData();
            formData.append("Name", this.name);
            formData.append("Description", this.description);
            
            this.$http.post("/api/group/", formData).catch(
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