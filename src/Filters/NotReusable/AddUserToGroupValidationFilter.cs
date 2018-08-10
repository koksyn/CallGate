using System.Linq;
using CallGate.ApiModels.GroupUser;
using CallGate.DependencyInjection;
using CallGate.Models;
using CallGate.Services.Group;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CallGate.Filters.NotReusable
{
    public class AddUserToGroupValidationFilter : IActionFilterDependency
    {
        private readonly IGroupValidationService _groupValidationService;

        public AddUserToGroupValidationFilter(
            IGroupValidationService groupValidationService
        ){
            _groupValidationService = groupValidationService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach(var argument in context.ActionArguments.Values.Where(v => v is AddUserToGroupRequest))
            {
                AddUserToGroupRequest command = argument as AddUserToGroupRequest;
                
                if (command.Role == Role.Admin)
                {
                    _groupValidationService.RequireAuthorizedUserIsGroupAdmin(command.GroupId);
                }
                
                _groupValidationService.RequireUsernameIsNotGroupMember(command.Username, command.GroupId);
            }
        }
        
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}