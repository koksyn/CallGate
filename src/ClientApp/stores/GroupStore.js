import CacheService from "../services/CacheService";
import TypeCheckService from "../services/utils/TypeCheckService";

export default {
    data: function () {
        return {
            groups: [],
            activeGroup: {},
            roleInActiveGroup: "Member"
        }
    },
    methods: {
        isActiveGroupNotEmpty: function () {
            return !TypeCheckService.isEmptyObject(this.activeGroup);
        },
        userHaveNotGroups: function () {
            return this.groups.length === 0;
        },
        getGroupsPromise: function () {
            return this.$http.get("/api/group");
        },
        getRoleInActiveGroupPromise: function () {
            return this.$http.get("/api/group/" + this.activeGroup.id + "/user/authorized/role");
        },
        computeActiveGroup: function () {
            let groupsExists = (this.groups.length > 0);
            
            if (groupsExists) {
                if (CacheService.hasActiveGroupId()) {
                    this.setActiveGroupFromCache();
                } else {
                    this.setDefaultActiveGroup();
                }
            } else {
                CacheService.clear();
                this.activeGroup = {};
            }
        },
        setActiveGroupFromCache: function () {
            let indexOfActiveGroup = this.findIndexOfActiveGroup();
            let notFound = (indexOfActiveGroup === -1);

            if (notFound) {
                this.setDefaultActiveGroup();
            } else {
                this.activeGroup = this.groups[indexOfActiveGroup];
            }   
        },
        findIndexOfActiveGroup: function () {
            let activeGroupId = CacheService.getActiveGroupId();
            
            return this.groups.findIndex(group => group.id === activeGroupId);
        },
        setDefaultActiveGroup: function () {
            this.activeGroup = this.groups[0];
            CacheService.setActiveGroupId(this.activeGroup.id);
        }
    }
}