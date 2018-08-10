using CallGate.ApiModels.ChatUser;
using CallGate.Filters;
using CallGate.Filters.NotReusable;
using CallGate.Services.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CallGate.Controllers
{
    [Authorize]
    [Route("api/chat/{chatId}/user")]
    [ApiModelValidationFilter]
    [ServiceFilter(typeof(ChatMemberFilter))]
    public class ChatUserController : Controller
    {
        private readonly IChatUserService _chatUserService;

        public ChatUserController(IChatUserService chatUserService)
        {
            _chatUserService = chatUserService;
        }
        
        [HttpGet]
        public IActionResult GetAllChatUsers(GetChatUsersRequest command)
        {
            var users = _chatUserService.GetAllChatUsers(command.ChatId, command.Username);

            return Json(users);
        }
        
        [HttpGet("groupUsersOutsideChat")]
        public IActionResult GetGroupUsersOutsideChat(GetGroupUsersOutsideChatRequest command)
        {
            var users = _chatUserService.GetGroupUsersOutsideChat(command.Username, command.ChatId);

            return Json(users);
        }
        
        [HttpPost]
        [ServiceFilter(typeof(AddUserToChatValidationFilter))]
        [ServiceFilter(typeof(TransactionFilter))]
        public IActionResult AddUserToChat(AddUserToChatRequest command)
        {
            _chatUserService.AddUserToChatByUsername(command.Username, command.ChatId);
                            
            return NoContent();
        }
    }
}