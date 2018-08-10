using System;
using System.ComponentModel.DataAnnotations;

namespace CallGate.ApiModels.Channel
{
    public class ChannelRequest
    {
        [Required]
        public Guid ChannelId { get; set; }
    }
}