using System.Linq;
using CallGate.ApiModels.Authentication;
using CallGate.DependencyInjection;
using CallGate.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CallGate.Filters.NotReusable
{
    public class RegisterValidationFilter : IActionFilterDependency
    {
        private readonly IUserValidationService _userValidationService;

        public RegisterValidationFilter(IUserValidationService userValidationService)
        {
            _userValidationService = userValidationService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach(var argument in context.ActionArguments.Values.Where(v => v is RegisterRequest))
            {
                RegisterRequest command = argument as RegisterRequest;

                _userValidationService.RequireUniqueUsernameAndEmail(command.Username, command.Email, context.ModelState);
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