using System;
using CallGate.DependencyInjection;

namespace CallGate.Services.Group
{
    public interface IGroupValidationService : ITransientDependency
    {
        void RequireUsernameIsGroupMember(string username, Guid groupId);
        
        void RequireUsernameIsNotGroupMember(string username, Guid groupId);
        
        void RequireUserIdIsGroupAdmin(Guid userId, Guid groupId, string errorMessage);
        
        void RequireUserIdIsGroupMember(Guid userId, Guid groupId, string errorMessage);
        
        void RequireUserIdIsNotGroupMember(Guid userId, Guid groupId);

        void RequireAuthorizedUserIsGroupAdmin(Guid groupId);
        
        void RequireAuthorizedUserIsGroupMember(Guid groupId);
    }
}