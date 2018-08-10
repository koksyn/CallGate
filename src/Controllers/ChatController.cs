using CallGate.ApiModels.Chat;
using CallGate.Filters;
using CallGate.Filters.NotReusable;
using CallGate.Services.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CallGate.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiModelValidationFilter]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }
        
        [HttpPost]
        [ServiceFilter(typeof(GroupMemberFilter))]
        [ServiceFilter(typeof(ChatCreateValidationFilter))]
        [ServiceFilter(typeof(TransactionFilter))]
        public IActionResult Create(ChatCreateRequest command)
        {   
            var chat = _chatService.Create(command.GroupId, command.Username);

            return Created($"/api/chat/{@chat.Id}", null);
        }
    }
}