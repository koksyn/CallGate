export default {
    data: function () {
        return {
            users: [],
            usersOutsidePrivateChats: [],
            detailedGroupUsers: []
        }
    },
    methods: {
        getUsersPromise: function () {
            return this.$http.get("/api/group/" + this.activeGroup.id + "/user");
        },
        getUsersOutsidePrivateChatsPromise: function () {
            return this.$http.get("/api/group/" + this.activeGroup.id + "/user/allUsersOutsideConnectedChats?chatUsersCount=2");
        },
        getDetailedGroupUsersPromise: function () {
            return this.$http.get("/api/group/" + this.activeGroup.id + "/user/details");
        },
        getUserByIdOutsidePrivateChats(userId) {
            let foundUser = null;

            this.usersOutsidePrivateChats.some(function (user) {
                if (user.id === userId) {
                    foundUser = user;
                    return true; // only way to "break" the loop
                }
            });

            return foundUser;
        }
    }
}