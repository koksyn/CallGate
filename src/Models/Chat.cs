using System;
using System.Collections.Generic;
using EntityFrameworkCore.Triggers;

namespace CallGate.Models
{
    public class Chat : BaseEntity
    {
        public Guid GroupId { get; set; }
        
        public Group Group { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public ICollection<ChatUser> ChatUsers { get; set; } = new List<ChatUser>();
        
        public Chat()
        {
            Triggers<Chat>.Inserting += entry => entry.Entity.CreatedAt = DateTime.UtcNow;
        }
    }
}