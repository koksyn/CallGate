import CacheService from "../services/CacheService";

export default {
    methods: {
        activateGroupById: function (groupId) {
            let indexOfGroup = this.groups.findIndex(group => group.id === groupId);

            if (indexOfGroup !== -1) {
                this.activateGroup(this.groups[indexOfGroup]);
            }
        },
        activateGroup: function (group) {
            if (group.id !== this.activeGroup.id) {
                this.activeGroup = group;
                CacheService.setActiveGroupId(group.id);
                this.reloadAsyncData();

                // when changing a group always redirect to group home page
                if(this.$route.fullPath !== '/') {
                    this.$router.push('/');
                }
            }
        }
    }
}