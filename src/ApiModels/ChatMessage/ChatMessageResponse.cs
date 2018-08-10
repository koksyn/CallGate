using System;

namespace CallGate.ApiModels.ChatMessage
{
    public class ChatMessageResponse
    {
        public Guid Id { get; set; }

        public string Content { get; set; }

        public DateTime Created { get; set; }
        
        public Guid ChatId { get; set; }
        
        public Guid UserId { get; set; }
        
        public string Username { get; set; }
    }
}