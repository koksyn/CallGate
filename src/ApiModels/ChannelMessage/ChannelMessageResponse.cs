using System;
using CallGate.ApiModels.User;

namespace CallGate.ApiModels.ChannelMessage
{
    public class ChannelMessageResponse
    {
        public Guid Id { get; set; }

        public string Content { get; set; }

        public DateTime Created { get; set; }
        
        public Guid ChannelId { get; set; }
        
        public Guid UserId { get; set; }
        
        public string Username { get; set; }
    }
}