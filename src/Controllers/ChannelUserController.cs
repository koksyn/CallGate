using CallGate.ApiModels.ChannelUser;
using CallGate.Filters;
using CallGate.Filters.NotReusable;
using CallGate.Services.Channel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CallGate.Controllers
{
    [Authorize]
    [Route("api/channel/{channelId}/user")]
    [ApiModelValidationFilter]
    [ServiceFilter(typeof(TransactionFilter))]
    public class ChannelUserController : Controller
    {
        private readonly IChannelUserService _channelUserService;

        public ChannelUserController(IChannelUserService channelUserService)
        {
            _channelUserService = channelUserService;
        }
        
        [HttpGet]
        [ServiceFilter(typeof(GetChannelUsersValidationFilter))]
        public IActionResult GetAllChannelUsers(GetChannelUsersRequest command)
        {
            var users = _channelUserService.GetAllChannelUsers(command.ChannelId, command.Username);

            return Json(users);
        }
        
        [HttpGet("groupUsersOutsideChannel")]
        [ServiceFilter(typeof(ChannelMemberFilter))]
        public IActionResult GetGroupUsersOutsideChannel(GetGroupUsersOutsideChannelRequest command)
        {
            var users = _channelUserService.GetGroupUsersOutsideChannel(command.Username, command.ChannelId);

            return Json(users);
        }
        
        [HttpPost("authorized")]
        [ServiceFilter(typeof(AddAuthorizedUserToChannelValidationFilter))]
        public IActionResult AddAuthorizedUserToChannel(AddAuthorizedUserToChannelRequest command)
        {
            _channelUserService.AddAuthorizedUserToChannel(command.ChannelId);
                            
            return NoContent();
        }
        
        [HttpPost]
        [ServiceFilter(typeof(ChannelMemberFilter))]
        [ServiceFilter(typeof(AddUserToChannelValidationFilter))]
        public IActionResult AddUserToChannel(AddUserToChannelRequest command)
        {
            _channelUserService.AddUserToChannelByUsername(command.Username, command.ChannelId);
                            
            return NoContent();
        }
        
        [HttpDelete("authorized")]
        [ServiceFilter(typeof(ChannelMemberFilter))]
        public IActionResult RemoveAuthorizedUserFromChannel(RemoveAuthorizedUserFromChannelRequest command)
        {
            _channelUserService.RemoveAuthorizedUserFromChannel(command.ChannelId);
            
            return NoContent();
        }
        
        [HttpDelete]
        [ServiceFilter(typeof(RemoveUserFromChannelValidationFilter))]
        public IActionResult RemoveUserFromChannel(RemoveUserFromChannelRequest command)
        {
            _channelUserService.RemoveUserFromChannelByUsername(command.Username, command.ChannelId);
            
            return NoContent();
        }
    }
}