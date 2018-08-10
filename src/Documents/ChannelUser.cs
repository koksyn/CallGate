using System;

namespace CallGate.Documents
{
    public class ChannelUser : BaseDocument
    {
        public ChannelUser(Guid userId, Guid channelId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            ChannelId = channelId;
        }
        
        public Guid UserId { get; set; }
        
        public Guid ChannelId { get; set; }
    }
}