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
    mounted: function () {
        if (this.$parent.isActiveGroupNotEmpty()) {
            this.updateGroupData();
        }

        this.$events.on('AsyncDataReloaded', () => this.updateGroupData());
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
        updateGroupData: function () {
            this.name = this.$parent.activeGroup.name;
            this.description = this.$parent.activeGroup.description;
        },
        editGroup: function () {
            this.errors = [];
            this.validate();

            if (this.errors.length === 0)
            {
                let formData = new FormData();
                formData.append("Name", this.name);
                formData.append("Description", this.description);

                this.$http.patch("/api/group/" + this.$parent.activeGroup.id, formData).catch(
                    (error) => {
                        if (this.isValidationError(error)) {
                            this.errors = BasicValidationService.getArrayOfErrorsFromResponse(error);
                        } else {
                            this.handleHttpNotValidationErrors(error);
                        }
                    }
                );
            }
        },
        validate: function() {
            if (this.name === "") {
                this.errors.push("Name cannot be empty.");
            }
            if (this.description === "") {
                this.errors.push("Description cannot be empty.");
            }
            
            if ((this.name === this.$parent.activeGroup.name) && 
                (this.description === this.$parent.activeGroup.description)) {
                this.errors.push("All fields are the same as before. Please change some of them to edit group.");
            }
        }
    }
}