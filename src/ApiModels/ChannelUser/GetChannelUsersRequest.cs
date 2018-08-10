using System.ComponentModel.DataAnnotations;
using CallGate.ApiModels.Channel;
using Microsoft.AspNetCore.Mvc;

namespace CallGate.ApiModels.ChannelUser
{
    public class GetChannelUsersRequest : ChannelRequest
    {
        [FromQuery(Name = "username")]
        [StringLength(255)]
        public string Username { get; set; }
    }
}