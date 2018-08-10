using System.ComponentModel.DataAnnotations;
using CallGate.ApiModels.Channel;

namespace CallGate.ApiModels.ChannelUser
{
    public class AddUserToChannelRequest : ChannelRequest
    {
        [Required]
        [StringLength(255)]
        public string Username { get; set; }
    }
}