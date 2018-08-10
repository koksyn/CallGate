using System;

namespace CallGate.Models
{
    public class ChatUser : IEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid ChatId { get; set; }
        public Chat Chat { get; set; }

        public ChatUser() {}

        public ChatUser(User user, Chat chat) 
        {
            User = user;
            Chat = chat;
        }
    }
}