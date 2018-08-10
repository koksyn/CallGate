using System.Linq;
using CallGate.ApiModels.ChannelUser;
using CallGate.DependencyInjection;
using CallGate.Services.Channel;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CallGate.Filters.NotReusable
{
    public class AddUserToChannelValidationFilter : IActionFilterDependency
    {
        private readonly IChannelValidationService _channelValidationService;

        public AddUserToChannelValidationFilter(
            IChannelValidationService channelValidationService
        ){
            _channelValidationService = channelValidationService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach(var argument in context.ActionArguments.Values.Where(v => v is AddUserToChannelRequest))
            {
                AddUserToChannelRequest command = argument as AddUserToChannelRequest;
                
                _channelValidationService.RequireUserIsGroupMemberFromChannel(command.Username, command.ChannelId);
                _channelValidationService.RequireUserIsNotChannelMember(command.Username, command.ChannelId);
            }
        }
        
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}