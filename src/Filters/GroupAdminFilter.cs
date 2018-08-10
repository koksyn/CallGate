using System.Linq;
using CallGate.ApiModels.Group;
using CallGate.DependencyInjection;
using CallGate.Services.Group;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CallGate.Filters
{
    public class GroupAdminFilter : IActionFilterDependency
    {
        private readonly IGroupValidationService _groupValidationService;

        public GroupAdminFilter(IGroupValidationService groupValidationService)
        {
            _groupValidationService = groupValidationService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach(var argument in context.ActionArguments.Values.Where(v => v is GroupRequest))
            {
                GroupRequest model = argument as GroupRequest;
                
                _groupValidationService.RequireAuthorizedUserIsGroupAdmin(model.GroupId);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}