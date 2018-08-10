using System.Linq;
using CallGate.ApiModels.ChatUser;
using CallGate.DependencyInjection;
using CallGate.Services.Chat;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CallGate.Filters.NotReusable
{
    public class AddUserToChatValidationFilter : IActionFilterDependency
    {
        private readonly IChatValidationService _chatValidationService;

        public AddUserToChatValidationFilter(
            IChatValidationService chatValidationService
        ){
            _chatValidationService = chatValidationService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach(var argument in context.ActionArguments.Values.Where(v => v is AddUserToChatRequest))
            {
                AddUserToChatRequest command = argument as AddUserToChatRequest;
                
                _chatValidationService.RequireUserIsGroupMemberFromChat(command.Username, command.ChatId);
                _chatValidationService.RequireUserIsNotChatMember(command.Username, command.ChatId);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}