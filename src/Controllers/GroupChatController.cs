using CallGate.ApiModels.Group;
using CallGate.ApiModels.GroupChat;
using CallGate.Filters;
using CallGate.Services.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CallGate.Controllers
{
    [Authorize]
    [Route("api/group/{groupId}/chat")]
    [ApiModelValidationFilter]
    [ServiceFilter(typeof(GroupMemberFilter))]
    public class GroupChatController : Controller
    {
        private readonly IChatService _chatService;

        public GroupChatController(IChatService chatService)
        {
            _chatService = chatService;
        }
        
        [HttpGet]
        public IActionResult GetGroupChatsForLoggedUser(GetGroupChatsForLoggedUserRequest command)
        {
            var chats = _chatService.GetGroupChatsForLoggedUser(command.GroupId);
            
            return Json(chats);
        }
        
        [HttpGet("count")]
        public IActionResult GetGroupChatsCount(GroupRequest command)
        {
            var count = _chatService.GetGroupChatsCount(command.GroupId);
            
            return Json(count);
        }
    }
}