using CallGate.ApiModels.ChatMessage;
using CallGate.Filters;
using CallGate.Services.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CallGate.Controllers
{
    [Authorize]
    [Route("api/chat/{chatId}/message")]
    [ApiModelValidationFilter]
    [ServiceFilter(typeof(ChatMemberFilter))]
    public class ChatMessageController : Controller
    {
        private readonly IChatMessageService _chatMessageService;

        public ChatMessageController(IChatMessageService chatMessageService)
        {
            _chatMessageService = chatMessageService;
        }

        [HttpGet]
        public IActionResult GetAllChatMessages(GetChatMessagesRequest command)
        {
            var messages = _chatMessageService.GetAllByChatId(command.ChatId);

            return Json(messages);
        }
    }
}