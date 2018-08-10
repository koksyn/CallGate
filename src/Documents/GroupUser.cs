using System;

namespace CallGate.Documents
{
    public class GroupUser : BaseDocument
    {
        public GroupUser(Guid userId, Guid groupId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            GroupId = groupId;
        }

        public Guid UserId { get; set; }
        
        public Guid GroupId { get; set; }
    }
}