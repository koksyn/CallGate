using CallGate.ApiModels.Channel;
using CallGate.Filters;
using CallGate.Services.Channel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CallGate.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiModelValidationFilter]
    public class ChannelController : Controller
    {
        private readonly IChannelService _channelService;

        public ChannelController(IChannelService channelService)
        {
            _channelService = channelService;
        }
        
        [HttpPost]
        [ServiceFilter(typeof(GroupMemberFilter))]
        [ServiceFilter(typeof(TransactionFilter))]
        public IActionResult Create(ChannelCreateRequest command)
        {   
            var channel = _channelService.Create(command.GroupId, command.Name);
           
            return Created($"/api/channel/{@channel.Id}", null);
        }
        
        [HttpGet("{ChannelId}")]
        [ServiceFilter(typeof(ChannelMemberFilter))]
        public IActionResult Get(ChannelRequest command)
        {
            var channel = _channelService.Get(command.ChannelId);

            if(channel == null)
            {
                return NotFound();
            }

            return Json(channel);
        }
    }
}