using System.Collections.Generic;

namespace CallGate.Models
{
    public class User : BaseEntity
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public string ConfirmationPhrase { get; set; }

        public ICollection<GroupUser> GroupUsers { get; } = new List<GroupUser>();
        
        public ICollection<ChatUser> ChatUsers { get; } = new List<ChatUser>();
        
        public ICollection<ChannelUser> ChannelUsers { get; } = new List<ChannelUser>();
    }
}
