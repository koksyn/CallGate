export const EventTypes = {
    // Group
    GroupCreated: 100,
    GroupEdited: 101,
    UserAddedToGroup: 110,
    UserRemovedFromGroup: 111,
    MemberRoleInGroupGranted: 120,
    AdminRoleInGroupGranted: 121,

    // Channel
    ChannelCreated: 200,
    ChannelRemoved: 202,
    UserAddedToChannel: 210,
    UserRemovedFromChannel: 211,

    // Chat
    ChatCreated: 300,
    ChatRemoved: 302,
    UserAddedToChat: 310,
    UserRemovedFromChat: 311
};

export default {
    data: function () {
        return {
            events: [],
            unreadedEventsCount: 0
        }
    },
    mounted: function () {
        this.$events.on('SocketEvent', (event) => this.addEvent(event));
    },
    watch: {
        'events': function () {
            Waves.attach('#eventsFeed .dropdown-menu li a', ['waves-block']);
            Waves.init();
        }
    },
    computed: {
        sortedEvents: function () {
            return this.events.sort((one, another) => new Date(one.created) < new Date(another.created));
        }
    },
    methods: {
        addEvent: function (event) {
            if (this.events.filter(e => e.id === event.id).length === 0) {
                this.events.unshift(event);
                this.unreadedEventsCount++;
            }
        },
        getEventsPromise: function () {
            return this.$http.get("/api/event/authorized");
        },
        resetUnreadedEventsCount: function () {
            this.unreadedEventsCount = 0;
        },
        getColorForEvent: function (event) {
            switch (event.type) {
                case EventTypes.GroupCreated:
                case EventTypes.ChannelCreated:
                case EventTypes.ChatCreated:
                case EventTypes.UserAddedToGroup:
                case EventTypes.UserAddedToChannel:
                case EventTypes.UserAddedToChat:
                    return "bg-indigo";
                case EventTypes.ChannelRemoved:
                case EventTypes.ChatRemoved:
                case EventTypes.UserRemovedFromGroup:
                case EventTypes.UserRemovedFromChannel:
                case EventTypes.UserRemovedFromChat:
                    return "bg-deep-purple";
                case EventTypes.GroupEdited:
                case EventTypes.MemberRoleInGroupGranted:
                case EventTypes.AdminRoleInGroupGranted:
                    return "bg-green";
                default:
                    return "bg-blue-grey";
            }
        },
        getIconNameForEvent: function (event) {
            switch (event.type) {
                case EventTypes.GroupCreated:
                    return "assignment_turned_in";
                case EventTypes.ChannelCreated:
                    return "question_answer";
                case EventTypes.ChatCreated:
                    return "group_add";
                case EventTypes.UserAddedToGroup:
                case EventTypes.UserAddedToChannel:
                case EventTypes.UserAddedToChat:
                    return "person_add";
                case EventTypes.ChannelRemoved:
                case EventTypes.ChatRemoved:
                case EventTypes.UserRemovedFromGroup:
                case EventTypes.UserRemovedFromChannel:
                case EventTypes.UserRemovedFromChat:
                    return "delete_forever";
                case EventTypes.GroupEdited:
                    return "cached";
                case EventTypes.MemberRoleInGroupGranted:
                    return "star_half";
                case EventTypes.AdminRoleInGroupGranted:
                    return "star";
                default:
                    return "cached";
            }
        },
        getAdaptedContentForEvent: function (event) {         
            // max 40+3(...)
            switch (event.type) {
                case EventTypes.GroupCreated:
                    return "Group " + this.adaptContent(event.groupName, 26) + " created";
                case EventTypes.ChannelCreated:
                    return "Channel " + this.adaptContent(event.channelName, 29) + " created";
                case EventTypes.ChatCreated:
                    return "Private chat created";
                case EventTypes.UserAddedToGroup:
                    return "User " + this.adaptContent(event.username, 12) + " added to group " + this.adaptContent(event.groupName, 12);
                case EventTypes.UserAddedToChannel:
                    return "User " + this.adaptContent(event.username, 11) + " added to channel " + this.adaptContent(event.channelName, 11);
                case EventTypes.UserAddedToChat:
                    return "User " + this.adaptContent(event.username, 26) + " added to chat";
                case EventTypes.ChannelRemoved:
                    return "Channel " + this.adaptContent(event.channelName, 30) + " removed";
                case EventTypes.ChatRemoved:
                    return "Private chat removed";
                case EventTypes.UserRemovedFromGroup:
                    return "User " + this.adaptContent(event.username, 10) + " removed from group " + this.adaptContent(event.groupName, 10);
                case EventTypes.UserRemovedFromChannel:
                    return "User " + this.adaptContent(event.username, 9) + " removed from channel " + this.adaptContent(event.channelName, 9);
                case EventTypes.UserRemovedFromChat:
                    return "User " + this.adaptContent(event.username, 22) + " removed from chat";
                case EventTypes.GroupEdited:
                    return "Group " + this.adaptContent(event.groupName, 27) + " was edited";
                case EventTypes.MemberRoleInGroupGranted:
                    return "Member role for " + this.adaptContent(event.username, 6) + " granted in group " + this.adaptContent(event.groupName, 5);
                case EventTypes.AdminRoleInGroupGranted:
                    return "Admin role for " + this.adaptContent(event.username, 6) + " granted in group " + this.adaptContent(event.groupName, 6);
                default:
                    return "Unknown event";
            }
        },
        adaptContent: function (content, maxLength) {
            if (content.length > maxLength) {
                content = content.slice(0, maxLength) + '\u2026';
            }
            
            return content;
        }
    }
}