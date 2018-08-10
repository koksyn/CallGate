using CallGate.ApiModels.Group;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CallGate.ApiModels.GroupUser;
using CallGate.Filters;
using CallGate.Filters.NotReusable;
using CallGate.Services.Group;

namespace CallGate.Controllers
{
    [Authorize]
    [Route("api/group/{groupId}/user")]
    [ApiModelValidationFilter]
    [ServiceFilter(typeof(GroupMemberFilter))]
    public class GroupUserController : Controller
    {
        private readonly IGroupUserService _groupUserService;

        public GroupUserController(IGroupUserService groupUserService)
        {
            _groupUserService = groupUserService;
        }

        [HttpGet]
        public IActionResult GetAllGroupMembers(GetGroupUsersRequest command)
        {
            var users = _groupUserService.GetAllGroupMembers(command.GroupId, command.Username);

            return Json(users);
        }
        
        [HttpGet("details")]
        [ServiceFilter(typeof(GroupAdminFilter))]
        public IActionResult GetAllGroupMembersDetails(GetGroupUsersRequest command)
        {
            var users = _groupUserService.GetAllGroupMembersDetails(command.GroupId, command.Username);

            return Json(users);
        }
        
        [HttpGet("allUsersOutsideGroup")]
        [ServiceFilter(typeof(GetAllUsersOutsideGroupValidationFilter))]
        public IActionResult GetAllUsersOutsideGroup(GetGroupUsersRequest command)
        {
            var users = _groupUserService.GetAllUsersOutsideGroup(command.GroupId, command.Username);

            return Json(users);
        }
        
        [HttpGet("allUsersOutsideConnectedChats")]
        public IActionResult GetAllUsersOutsideConnectedChats(GetUsersOutsideConnectedChatsRequest command)
        {
            var users = _groupUserService.GetAllUsersOutsideConnectedChats(command.GroupId, command.ChatUsersCount);

            return Json(users);
        }
        
        [HttpGet("authorized/role")]
        public IActionResult GetAuthorizedUserRoleInGroup(GroupRequest command)
        {
            var role = _groupUserService.GetAuthorizedUserRoleNameInGroup(command.GroupId);
                
            return Json(role);
        }
        
        [HttpPost]
        [ServiceFilter(typeof(AddUserToGroupValidationFilter))]
        [ServiceFilter(typeof(TransactionFilter))]
        public IActionResult AddUserToGroup(AddUserToGroupRequest command)
        {
            _groupUserService.AddUserToGroup(command.Username, command.Role, command.GroupId);
                            
            return NoContent();
        }
        
        [HttpPut]
        [ServiceFilter(typeof(GroupAdminFilter))]
        [ServiceFilter(typeof(EditGroupUserValidationFilter))]
        [ServiceFilter(typeof(TransactionFilter))]
        public IActionResult EditGroupUser(EditGroupUserRequest command)
        {
            _groupUserService.EditGroupUser(command.Username, command.Role, command.GroupId);
                            
            return NoContent();
        }
        
        [HttpDelete("authorized")]
        [ServiceFilter(typeof(TransactionFilter))]
        public IActionResult RemoveAuthorizedUserFromGroup(RemoveAuthorizedUserFromGroupRequest command)
        {
            _groupUserService.RemoveAuthorizedUserFromGroup(command.GroupId);
            
            return NoContent();
        }
        
        [HttpDelete]
        [ServiceFilter(typeof(GroupAdminFilter))]
        [ServiceFilter(typeof(RemoveUserFromGroupValidationFilter))]
        [ServiceFilter(typeof(TransactionFilter))]
        public IActionResult RemoveUserFromGroup(RemoveUserFromGroupRequest command)
        {
            _groupUserService.RemoveUserFromGroupByUsername(command.Username, command.GroupId);
            
            return NoContent();
        }
    }
}