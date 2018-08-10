using CallGate.ApiModels.ChannelMessage;
using CallGate.Filters;
using CallGate.Services.Channel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CallGate.Controllers
{
    [Authorize]
    [Route("api/channel/{channelId}/message")]
    [ApiModelValidationFilter]
    [ServiceFilter(typeof(ChannelMemberFilter))]
    public class ChannelMessageController : Controller
    {
        private readonly IChannelMessageService _channelMessageService;

        public ChannelMessageController(IChannelMessageService channelMessageService)
        {
            _channelMessageService = channelMessageService;
        }

        [HttpGet]
        public IActionResult GetAllChannelMessages(GetChannelMessagesRequest command)
        {
            var messages = _channelMessageService.GetAllByChannelId(command.ChannelId);

            return Json(messages);
        }
    }
}