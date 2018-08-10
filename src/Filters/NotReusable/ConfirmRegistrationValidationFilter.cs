using System.Linq;
using CallGate.ApiModels.Authentication;
using CallGate.DependencyInjection;
using CallGate.Services.User;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CallGate.Filters.NotReusable
{
    public class ConfirmRegistrationValidationFilter : IActionFilterDependency
    {
        private readonly IUserValidationService _userValidationService;

        public ConfirmRegistrationValidationFilter(IUserValidationService userValidationService)
        {
            _userValidationService = userValidationService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach(var argument in context.ActionArguments.Values.Where(v => v is ConfirmRegistrationRequest))
            {
                ConfirmRegistrationRequest command = argument as ConfirmRegistrationRequest;

                _userValidationService.RequireValidConfirmationPhraseForInactiveUser(command.ConfirmationPhrase);
            }
        }
        
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}