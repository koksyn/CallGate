export default {
    data: function () {
        return {
            connectedChats: [],
            chatsCount: 0
        }
    },
    methods: {
        getConnectedChatsPromise: function () {
            return this.$http.get("/api/group/" + this.activeGroup.id + "/chat");
        },
        getChatsCountPromise: function () {
            return this.$http.get("/api/group/" + this.activeGroup.id + "/chat/count");
        }
    }
}