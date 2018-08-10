using System;
using System.ComponentModel.DataAnnotations;

namespace CallGate.ApiModels.Chat
{
    public class ChatRequest
    {
        [Required]
        public Guid ChatId { get; set; }
    }
}