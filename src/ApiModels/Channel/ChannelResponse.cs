using System;

namespace CallGate.ApiModels.Channel
{
    public class ChannelResponse
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}