using System;
using CallGate.Repositories;
using CallGate.Exceptions;
using CallGate.Models;
using CallGate.Services.Helper;
using CallGate.Services.User;

namespace CallGate.Services.Group
{
    public class GroupValidationService : IGroupValidationService
    {
        private readonly IGroupUserRepository _groupUserRepository;
        private readonly Models.User _authorizedUser;
        private readonly IUserValidationService _userValidationService;

        public GroupValidationService(
            IAuthorizedUserHelper authorizedUserHelper,
            IGroupUserRepository groupUserRepository,
            IUserValidationService userValidationService
        )
        {
            _groupUserRepository = groupUserRepository;
            _authorizedUser = authorizedUserHelper.GetAuthorizedUser();
            _userValidationService = userValidationService;
        }
        
        public void RequireUsernameIsGroupMember(string username, Guid groupId)
        {
            var user = _userValidationService.RequireAndGetUserByUsername(username);
                
            RequireUserIdIsGroupMember(
                user.Id, 
                groupId, 
                "User is not a member of this group."
            );
        }

        public void RequireUsernameIsNotGroupMember(string username, Guid groupId)
        {
            var user = _userValidationService.RequireAndGetUserByUsername(username);
                
            RequireUserIdIsNotGroupMember(user.Id, groupId);
        }

        public void RequireUserIdIsGroupAdmin(Guid userId, Guid groupId, string errorMessage)
        {
            var userGroup = _groupUserRepository.GetByUserIdAndGroupIdAndRole(userId, groupId, Role.Admin);
                
            if (userGroup == null)
            {
                throw new LogicApiException(errorMessage);
            }
        }

        public void RequireUserIdIsGroupMember(Guid userId, Guid groupId, string errorMessage)
        {
            var userGroup = _groupUserRepository.GetByUserIdAndGroupId(userId, groupId);
                
            if (userGroup == null)
            {
                throw new LogicApiException(errorMessage);
            }
        }

        public void RequireUserIdIsNotGroupMember(Guid userId, Guid groupId)
        {
            var userGroup = _groupUserRepository.GetByUserIdAndGroupId(userId, groupId);
                
            if (userGroup != null)
            {
                throw new LogicApiException("User is already member of this group.");
            }
        }

        public void RequireAuthorizedUserIsGroupAdmin(Guid groupId)
        {
            RequireAuthorizedUserHaveRoleInGroup(groupId, Role.Admin);
        }
        
        public void RequireAuthorizedUserIsGroupMember(Guid groupId)
        {
            RequireAuthorizedUserHaveRoleInGroup(groupId);
        }
        
        private void RequireAuthorizedUserHaveRoleInGroup(Guid groupId, Role? role = null)
        {
            var userGroup = _groupUserRepository.GetByUserIdAndGroupId(_authorizedUser.Id, groupId);
            
            if (userGroup == null)
            {
                throw new LogicApiException("You are not member of this Group.");
            }
            
            if (role != null && userGroup.Role != role)
            {
                throw new NotEnoughRightsApiException();
            }
        }
    }
}