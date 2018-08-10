using System.Linq;
using CallGate.ApiModels.Channel;
using CallGate.DependencyInjection;
using CallGate.Services.Channel;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CallGate.Filters
{
    public class ChannelMemberFilter : IActionFilterDependency
    {
        private readonly IChannelValidationService _channelValidationService;

        public ChannelMemberFilter(IChannelValidationService channelValidationService)
        {
            _channelValidationService = channelValidationService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach(var argument in context.ActionArguments.Values.Where(v => v is ChannelRequest))
            {
                ChannelRequest command = argument as ChannelRequest;
                
                _channelValidationService.RequireAuthorizedUserIsGroupMemberFromChannel(command.ChannelId);
                _channelValidationService.RequireAuthorizedUserIsChannelMember(command.ChannelId);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}