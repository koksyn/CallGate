using System;
using System.Collections.Generic;
using EntityFrameworkCore.Triggers;

namespace CallGate.Models
{
    public class Group : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
       
        public ICollection<GroupUser> GroupUsers { get; } = new List<GroupUser>();

        public Group()
        {
            Triggers<Group>.Inserting += entry => entry.Entity.CreatedAt = entry.Entity.UpdatedAt = DateTime.UtcNow;
            Triggers<Group>.Updating += entry => entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}