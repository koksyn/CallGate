import BasicValidationService from "../../services/utils/BasicValidationService";
import RoleChecker from "../../mixins/RoleChecker";
import DetailsPreloader from "../../mixins/DetailsPreloader";
import HttpErrorHandler from "../../mixins/HttpErrorHandler";

export default {
    mixins: [RoleChecker, DetailsPreloader, HttpErrorHandler],
    data: function () {
        return {
            activeGroupId: "",
            choosedUser: null,
            errors: [],
            liveSearch: {},
            foundUsers: [],
            role: "Member",
        };
    },
    mounted: function () {
        if (this.$parent.isActiveGroupNotEmpty()) {
            this.activeGroupId = this.$parent.activeGroup.id;
        }
        this.$events.on('AsyncDataReloaded', () => {
            this.activeGroupId = this.$parent.activeGroup.id;
        });
        
        $(this.$el).find('#rolepicker').selectpicker();
        this.liveSearch = $(this.$el).find('#user-search');

        this.liveSearch.selectpicker().on('changed.bs.select', () => {
            let index = this.liveSearch.val();

            this.choosedUser = this.foundUsers[index];
        });

        this.liveSearch.selectpicker('refresh');

        this.liveSearch.parent().on('keyup', (event) => {
            let phrase = event.target.value;

            if (phrase.length >= 3) {
                this.searchUsername(phrase);
            }
        });
    },
    watch: {
        'foundUsers': function () {
            let context = this;

            this.$nextTick(function () {
                context.liveSearch.selectpicker('refresh');
                context.liveSearch.selectpicker('render');
            });
        }
    },
    beforeRouteLeave: function (to, from, next) {
        this.liveSearch.selectpicker('destroy');
        next();
    },
    computed: {
        options() {
            return this.foundUsers;
        },
        classWhenError: function () {
            return {
                error: this.errors.length > 0,
                focused: this.errors.length > 0,
            }
        }
    },
    methods: {
        clearErrors: function () {
            this.errors = [];
            $("div.bootstrap-select").removeClass("error");
        },
        addUserToGroup: function () {
            this.clearErrors();
            
            if (this.choosedUser === null) {
                this.errors.push("You haven't got any username choosed!")
            } else {
                let formData = new FormData();
                formData.append("Username", this.choosedUser.username);
                
                if (this.isAdmin()) {
                    formData.append("Role", this.role);
                }
                
                this.$http.post("/api/group/" + this.activeGroupId + "/user", formData).then(
                    (response) => this.removeChoosedUser(),
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
        removeChoosedUser: function () {
            this.foundUsers = this.foundUsers.filter(
                item => item !== this.choosedUser
            );
            
            this.choosedUser = null;
        },
        searchUsername: function (phrase) {
            this.clearErrors();
            this.showDetailsPreloader();

            let queryParams = {
                params: {
                    username: phrase
                }
            };
            
            this.$http.get("/api/group/" + this.activeGroupId + "/user/allUsersOutsideGroup", queryParams).then(
                (response) => { 
                    this.foundUsers = response.body;
                    this.hideDetailsPreloader();
                },
                (error) => {
                    this.hideDetailsPreloader();
                    
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