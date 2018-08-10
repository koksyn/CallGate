using System.Linq;
using CallGate.ApiModels.Chat;
using CallGate.DependencyInjection;
using CallGate.Services.Chat;
using CallGate.Services.Group;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CallGate.Filters.NotReusable
{
    public class ChatCreateValidationFilter : IActionFilterDependency
    {
        private readonly IChatValidationService _chatValidationService;
        private readonly IGroupValidationService _groupValidationService;

        public ChatCreateValidationFilter(
            IChatValidationService chatValidationService,
            IGroupValidationService groupValidationService
        ){
            _chatValidationService = chatValidationService;
            _groupValidationService = groupValidationService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach(var argument in context.ActionArguments.Values.Where(v => v is ChatCreateRequest))
            {
                ChatCreateRequest command = argument as ChatCreateRequest;

                _chatValidationService.RequireDifferentUsersForChatCreation(command.Username);
                
                _groupValidationService.RequireUsernameIsGroupMember(command.Username, command.GroupId);
                
                _chatValidationService.RequireChatBetweenUsersInGroupDoesNotExist(command.Username, command.GroupId);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}