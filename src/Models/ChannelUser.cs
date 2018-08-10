using System;

namespace CallGate.Models
{
    public class ChannelUser : IEntity
    {
        public Guid UserId { get; set; }
        
        public User User { get; set; }
        
        public Guid ChannelId { get; set; }
        
        public Channel Channel { get; set; }

        public ChannelUser() {}

        public ChannelUser(User user, Channel channel) 
        {
            User = user;
            Channel = channel;
        }
    }
}