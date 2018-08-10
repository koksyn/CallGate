namespace CallGate.Documents
{
    public enum EventType
    {
        // Group
        GroupCreated = 100,
        GroupEdited = 101,
        UserAddedToGroup = 110,
        UserRemovedFromGroup = 111,
        MemberRoleInGroupGranted = 120,
        AdminRoleInGroupGranted = 121,
        
        // Channel
        ChannelCreated = 200,
        ChannelRemoved = 202,
        UserAddedToChannel = 210,
        UserRemovedFromChannel = 211,
        
        // Chat
        ChatCreated = 300,
        ChatRemoved = 302,
        UserAddedToChat = 310,
        UserRemovedFromChat = 311
    }
}