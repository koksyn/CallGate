using System.ComponentModel.DataAnnotations;
using CallGate.ApiModels.Channel;
using Microsoft.AspNetCore.Mvc;

namespace CallGate.ApiModels.ChannelUser
{
    public class RemoveUserFromChannelRequest : ChannelRequest
    {
        [Required]
        [StringLength(255)]
        [FromQuery(Name = "username")]
        public string Username { get; set; }
    }
}