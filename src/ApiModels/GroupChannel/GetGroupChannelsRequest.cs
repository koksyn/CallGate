using System.ComponentModel.DataAnnotations;
using CallGate.ApiModels.Group;
using Microsoft.AspNetCore.Mvc;

namespace CallGate.ApiModels.GroupChannel
{
    public class GetGroupChannelsRequest : GroupRequest
    {
        [FromQuery(Name = "channelName")]
        [StringLength(255)]
        public string ChannelName { get; set; }  
    }
}