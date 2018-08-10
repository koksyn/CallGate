export default {
    data: function () {
        return {
            channels: [],
            connectedChannels: [],
            notConnectedChannels: []
        }
    },
    methods: {
        getChannelsPromise: function () {
            return this.$http.get("/api/group/" + this.activeGroup.id + "/channel");
        },
        getConnectedChannelsPromise: function () {
            return this.$http.get("/api/group/" + this.activeGroup.id + "/channel/connected");
        },
        getNotConnectedChannelsPromise: function () {
            return this.$http.get("/api/group/" + this.activeGroup.id + "/channel/notConnected");
        }
    }
}