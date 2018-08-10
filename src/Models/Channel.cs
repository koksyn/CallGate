using System;
using System.Collections.Generic;
using EntityFrameworkCore.Triggers;

namespace CallGate.Models
{
    public class Channel : BaseEntity
    {
        public Guid GroupId { get; set; }

        public Group Group { get; set; }
        
        public string Name { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public ICollection<ChannelUser> ChannelUsers { get; } = new List<ChannelUser>();
        
        public Channel()
        {
            Triggers<Channel>.Inserting += entry => entry.Entity.CreatedAt = DateTime.UtcNow;
        }
    }
}
