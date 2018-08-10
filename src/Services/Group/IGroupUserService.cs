using System;
using System.Collections.Generic;
using CallGate.ApiModels.User;
using CallGate.DependencyInjection;
using CallGate.Models;

namespace CallGate.Services.Group
{
    public interface IGroupUserService : ITransientDependency
    {
        void AddUserToGroup(string username, Role role, Guid groupId);
        
        void EditGroupUser(string username, Role role, Guid groupId);
        
        void RemoveAuthorizedUserFromGroup(Guid groupId);

        void RemoveAuthorizedUserFromAssociatedGroups();
        
        void RemoveUserFromGroupByUsername(string username, Guid groupId);
        
        IEnumerable<UserResponse> GetAllGroupMembers(Guid groupId, string username);
        
        IEnumerable<UserResponse> GetAllUsersOutsideGroup(Guid groupId, string username);
        
        IEnumerable<UserDetailsResponse> GetAllGroupMembersDetails(Guid groupId, string username);
        
        IEnumerable<UserResponse> GetAllUsersOutsideConnectedChats(Guid groupId, int chatUsersCount);
        
        string GetAuthorizedUserRoleNameInGroup(Guid groupId);
        
        void RemoveUserFromGroup(Models.User user, Guid groupId);
    }
}