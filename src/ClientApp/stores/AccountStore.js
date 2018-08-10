export default {
    data: function () {
        return {
            account: {}
        }
    },
    methods: {
        getAuthorizedAccountPromise: function () {     
            return this.$http.get("/api/account/authorized");
        },
        isLoggedAccountId: function (userId) {
            return this.account.id === userId;
        }
    }
}