using System.Linq;
using CallGate.ApiModels.Chat;
using CallGate.DependencyInjection;
using CallGate.Services.Chat;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CallGate.Filters
{
    public class ChatMemberFilter : IActionFilterDependency
    {
        private readonly IChatValidationService _chatValidationService;

        public ChatMemberFilter(IChatValidationService chatValidationService)
        {
            _chatValidationService = chatValidationService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach(var argument in context.ActionArguments.Values.Where(v => v is ChatRequest))
            {
                ChatRequest command = argument as ChatRequest;
                
                _chatValidationService.RequireAuthorizedUserIsGroupMemberFromChat(command.ChatId);
                _chatValidationService.RequireAuthorizedUserIsChatMember(command.ChatId);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}