using CallGate.ApiModels.User;
using CallGate.Filters;
using CallGate.Filters.NotReusable;
using CallGate.Services.Group;
using CallGate.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CallGate.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiModelValidationFilter]
    public class AccountController : Controller
    {
        private readonly IGroupUserService _groupUserService;
        private readonly IUserService _userService;

        public AccountController(
            IGroupUserService groupUserService,
            IUserService userService
        ) {
            _groupUserService = groupUserService;
            _userService = userService;
        }
        
        [HttpGet("authorized")]
        public IActionResult GetAuthorizedUser()
        {
            var user = _userService.GetAuthorizedUser();

            return Json(user);
        }
        
        [HttpDelete("authorized")]
        [ServiceFilter(typeof(RemoveAuthorizedUserValidationFilter))]
        [ServiceFilter(typeof(TransactionFilter))]
        public IActionResult RemoveAuthorizedUser(RemoveAuthorizedUserRequest command)
        {
            _groupUserService.RemoveAuthorizedUserFromAssociatedGroups();
            _userService.RemoveAuthorizedUser();
            
            return NoContent();
        }
    }
}