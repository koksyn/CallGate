using System;

namespace CallGate.Documents
{
    public class ChatMessage : BaseDocument
    {
        public string Content { get; set; }

        public DateTime Created { get; set; }

        public Guid ChatId { get; set; }

        public Guid UserId { get; set; }

        public string Username { get; set; }
    }
}