using System;
using EntityFrameworkCore.Triggers;

namespace CallGate.Models
{
    public enum Role
    {
        Member, Admin
    }
    
    public class GroupUser : IEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid GroupId { get; set; }
        public Group Group { get; set; }
        public Role Role { get; set; }
        public DateTime JoinedAt { get; set; }

        public GroupUser() {}

        public GroupUser(User user, Group group, Role role = Role.Member) 
        {
            User = user;
            Group = group;
            Role = role;
            Triggers<GroupUser>.Inserting += entry => entry.Entity.JoinedAt = DateTime.UtcNow;
        }
    }
}