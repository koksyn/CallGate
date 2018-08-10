using System;
using CallGate.Models;

namespace CallGate.Documents
{
    public class Event : BaseDocument
    {
        public EventType Type { get; set; }

        public DateTime Created { get; set; }
        
        public Guid AuthorUserId { get; set; }
        
        public string AuthorUsername { get; set; }
        
        public Guid GroupId { get; set; }
        
        public string GroupName { get; set; }
        
        public Guid? ChannelId { get; set; }
        
        public string ChannelName { get; set; }
        
        public Guid? ChatId { get; set; }
        
        public Guid? UserId { get; set; }
        
        public string Username { get; set; }

        public Event() {}

        public Event(EventType type, User authorUser, Group group)
        {
            Id = Guid.NewGuid();
            Type = type;
            Created = DateTime.UtcNow;
            AuthorUserId = authorUser.Id;
            AuthorUsername = authorUser.Username;
            GroupId = group.Id;
            GroupName = group.Name;
        }

        public void AttachUser(User user)
        {
            UserId = user.Id;
            Username = user.Username;
        }
        
        public void AttachChannel(Channel channel)
        {
            ChannelId = channel.Id;
            ChannelName = channel.Name;
        }
        
        public void AttachChat(Chat chat)
        {
            ChatId = chat.Id;
        }
    }
}