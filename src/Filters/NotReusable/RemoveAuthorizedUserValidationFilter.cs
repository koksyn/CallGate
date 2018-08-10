using System.Linq;
using CallGate.ApiModels.User;
using CallGate.DependencyInjection;
using CallGate.Services.User;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CallGate.Filters.NotReusable
{
    public class RemoveAuthorizedUserValidationFilter : IActionFilterDependency
    {
        private readonly IUserValidationService _userValidationService;

        public RemoveAuthorizedUserValidationFilter(IUserValidationService userValidationService)
        {
            _userValidationService = userValidationService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach(var argument in context.ActionArguments.Values.Where(v => v is RemoveAuthorizedUserRequest))
            {
                RemoveAuthorizedUserRequest command = argument as RemoveAuthorizedUserRequest;

                _userValidationService.RequireValidPasswordForAuthorizedUser(command.Password);
            }
        }
        
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}