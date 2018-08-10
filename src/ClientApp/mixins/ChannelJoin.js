import HttpErrorHandler from "./HttpErrorHandler";

export default {
    mixins: [HttpErrorHandler],
    methods: {
        joinChannel: function (channel) {
            this.$http.post("/api/channel/" + channel.id + "/user/authorized").catch(
                (error) => this.handleHttpNotValidationErrors(error)
            );
        }
    }
}