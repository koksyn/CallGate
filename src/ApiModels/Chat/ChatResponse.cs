using System;
using System.Collections.Generic;
using CallGate.ApiModels.User;

namespace CallGate.ApiModels.Chat
{
    public class ChatResponse
    {
        public Guid Id { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public IEnumerable<UserResponse> Users { get; set; }
    }
}