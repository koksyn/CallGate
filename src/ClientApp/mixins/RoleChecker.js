export default {
    props: ['roleInActiveGroup'],
    data: function () {
        return {
            roles: {
                member: "Member",
                admin: "Admin"
            }
        }
    },
    methods: {
        isAdmin: function () {
            return this.roles.admin === this.roleInActiveGroup;
        }
    }
}