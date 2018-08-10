using CallGate.ApiModels.Authentication;
using CallGate.Filters;
using CallGate.Filters.NotReusable;
using Microsoft.AspNetCore.Mvc;
using IAuthenticationService = CallGate.Services.Authentication.IAuthenticationService;

namespace CallGate.Controllers
{
    [ApiModelValidationFilter]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [ServiceFilter(typeof(LoginValidationFilter))]
        public IActionResult Login(LoginRequest loginRequest)
        {
            var tokenResponse = _authenticationService.GenerateAuthenticationToken(loginRequest);
            
            return Json(tokenResponse);
        }

        [HttpPost]
        [ServiceFilter(typeof(RegisterValidationFilter))]
        [ServiceFilter(typeof(TransactionFilter))]
        public IActionResult Register(RegisterRequest registerRequest)
        {
            _authenticationService.Register(registerRequest);
            
            return NoContent();
        }

        [HttpPatch]
        [ServiceFilter(typeof(ConfirmRegistrationValidationFilter))]
        [ServiceFilter(typeof(TransactionFilter))]
        public IActionResult ConfirmRegistration(ConfirmRegistrationRequest confirmRegistrationRequest)
        {
            _authenticationService.ConfirmRegistration(confirmRegistrationRequest);
            
            return NoContent();
        }
    }
}