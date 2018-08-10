using System.Linq;
using CallGate.ApiModels.Authentication;
using CallGate.DependencyInjection;
using CallGate.Services.User;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CallGate.Filters.NotReusable
{
    public class LoginValidationFilter : IActionFilterDependency
    {
        private readonly IUserValidationService _userValidationService;

        public LoginValidationFilter(IUserValidationService userValidationService)
        {
            _userValidationService = userValidationService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach(var argument in context.ActionArguments.Values.Where(v => v is LoginRequest))
            {
                LoginRequest command = argument as LoginRequest;

                _userValidationService.RequireValidPasswordForExistingUsername(command.Password, command.Username);
            }
        }
        
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}