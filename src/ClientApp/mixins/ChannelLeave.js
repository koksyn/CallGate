import HttpErrorHandler from "./HttpErrorHandler";

export default {
    mixins: [HttpErrorHandler],
    methods: {
        leaveChannel: function (channel) {
            this.leaveChannelById(channel.id);
        },
        leaveChannelById: function (channelId) {
            this.$http.delete("/api/channel/" + channelId + "/user/authorized").catch(
                (error) => this.handleHttpNotValidationErrors(error)
            );
        }
    }
}