using System.Linq;
using CallGate.ApiModels.ChannelUser;
using CallGate.DependencyInjection;
using CallGate.Services.Channel;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CallGate.Filters.NotReusable
{
    public class AddAuthorizedUserToChannelValidationFilter : IActionFilterDependency
    {
        private readonly IChannelValidationService _channelValidationService;

        public AddAuthorizedUserToChannelValidationFilter(
            IChannelValidationService channelValidationService
        ){
            _channelValidationService = channelValidationService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach(var argument in context.ActionArguments.Values.Where(v => v is AddAuthorizedUserToChannelRequest))
            {
                AddAuthorizedUserToChannelRequest command = argument as AddAuthorizedUserToChannelRequest;

                _channelValidationService.RequireAuthorizedUserIsGroupMemberFromChannel(command.ChannelId);
                _channelValidationService.RequireAuthorizedUserIsNotChannelMember(command.ChannelId);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}