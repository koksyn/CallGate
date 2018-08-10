using CallGate.ApiModels.GroupChannel;
using CallGate.Filters;
using CallGate.Services.Channel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CallGate.Controllers
{
    [Authorize]
    [Route("api/group/{groupId}/channel")]
    [ApiModelValidationFilter]
    [ServiceFilter(typeof(GroupMemberFilter))]
    public class GroupChannelController : Controller
    {
        private readonly IChannelService _channelService;

        public GroupChannelController(IChannelService channelService)
        {
            _channelService = channelService;
        }
        
        [HttpGet]
        public IActionResult GetGroupChannels(GetGroupChannelsRequest command)
        {
            var channels = _channelService.GetChannelsForGroup(command.GroupId);
            
            return Json(channels);
        }
        
        [HttpGet("connected")]
        public IActionResult GetGroupChannelsConnectedWithLoggedUser(GetGroupChannelsRequest command)
        {
            var channels = _channelService.GetChannelsInGroupConnectedWithLoggedUser(command.GroupId, command.ChannelName);
            
            return Json(channels);
        }
        
        [HttpGet("notConnected")]
        public IActionResult GetGroupChannelsNotConnectedWithLoggedUser(GetGroupChannelsRequest command)
        {
            var channels = _channelService.GetChannelsInGroupNotConnectedWithLoggedUser(command.GroupId, command.ChannelName);
            
            return Json(channels);
        }
    }
}