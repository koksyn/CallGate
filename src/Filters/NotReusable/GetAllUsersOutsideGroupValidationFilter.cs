using System.Linq;
using CallGate.ApiModels.GroupUser;
using CallGate.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CallGate.Filters.NotReusable
{
    public class GetAllUsersOutsideGroupValidationFilter : IActionFilterDependency
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach(var argument in context.ActionArguments.Values.Where(v => v is GetGroupUsersRequest))
            {
                GetGroupUsersRequest command = argument as GetGroupUsersRequest;
                
                if (string.IsNullOrEmpty(command.Username) || command.Username.Length < 3)
                {
                    context.ModelState.AddModelError("Username", "You have to provide at least 3 characters");
                }
            }
            
            if (!context.ModelState.IsValid) {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
        
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}