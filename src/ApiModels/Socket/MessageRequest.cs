using System;

namespace CallGate.ApiModels.Socket
{
    public class MessageRequest
    {
        public Guid ChannelId { get; set; }
        public Guid ChatId { get; set; }
        public string Content { get; set; }
    }
}