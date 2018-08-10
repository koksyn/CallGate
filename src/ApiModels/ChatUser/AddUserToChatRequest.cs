using System.ComponentModel.DataAnnotations;
using CallGate.ApiModels.Chat;

namespace CallGate.ApiModels.ChatUser
{
    public class AddUserToChatRequest : ChatRequest
    {
        [Required]
        [StringLength(255)]
        public string Username { get; set; }
    }
}