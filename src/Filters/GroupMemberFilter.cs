using System.Linq;
using CallGate.ApiModels.Group;
using CallGate.DependencyInjection;
using CallGate.Services.Group;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CallGate.Filters
{
    public class GroupMemberFilter : IActionFilterDependency
    {
        private readonly IGroupValidationService _groupValidationService;

        public GroupMemberFilter(IGroupValidationService groupUserService)
        {
            _groupValidationService = groupUserService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach(var argument in context.ActionArguments.Values.Where(v => v is GroupRequest))
            {
                GroupRequest model = argument as GroupRequest;
                
                _groupValidationService.RequireAuthorizedUserIsGroupMember(model.GroupId);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}