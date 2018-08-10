using System.Linq;
using CallGate.ApiModels.ChannelUser;
using CallGate.DependencyInjection;
using CallGate.Services.Channel;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CallGate.Filters.NotReusable
{
    public class RemoveUserFromChannelValidationFilter : IActionFilterDependency
    {
        private readonly IChannelValidationService _channelValidationService;

        public RemoveUserFromChannelValidationFilter(IChannelValidationService channelValidationService)
        {
            _channelValidationService = channelValidationService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach(var argument in context.ActionArguments.Values.Where(v => v is RemoveUserFromChannelRequest))
            {
                RemoveUserFromChannelRequest command = argument as RemoveUserFromChannelRequest;

                _channelValidationService.RequireAuthorizedUserIsGroupAdminFromChannel(command.ChannelId);
                _channelValidationService.RequireUserIsGroupMemberFromChannel(command.Username, command.ChannelId);
                _channelValidationService.RequireUserIsChannelMember(command.Username, command.ChannelId);
            }
        }
        
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}