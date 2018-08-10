using System;

namespace CallGate.Documents
{
    public class ChatUser : BaseDocument
    {
        public ChatUser(Guid userId, Guid chatId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            ChatId = chatId;
        }

        public Guid UserId { get; set; }
        
        public Guid ChatId { get; set; }
    }
}