using System.Linq;
using CallGate.ApiModels.ChannelUser;
using CallGate.DependencyInjection;
using CallGate.Services.Channel;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CallGate.Filters.NotReusable
{
    public class GetChannelUsersValidationFilter : IActionFilterDependency
    {
        private readonly IChannelValidationService _channelValidationService;

        public GetChannelUsersValidationFilter(
            IChannelValidationService channelValidationService
        ){
            _channelValidationService = channelValidationService;
        }
        
        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach(var argument in context.ActionArguments.Values.Where(v => v is GetChannelUsersRequest))
            {
                GetChannelUsersRequest command = argument as GetChannelUsersRequest;
                
                _channelValidationService.RequireAuthorizedUserIsGroupMemberFromChannel(command.ChannelId);
            }
        }
        
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}