using System.ComponentModel.DataAnnotations;
using CallGate.ApiModels.Group;

namespace CallGate.ApiModels.Chat
{
    public class ChatCreateRequest : GroupRequest
    {
        [Required]
        [StringLength(255)]
        public string Username { get; set; }
    }
}