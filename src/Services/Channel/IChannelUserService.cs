using System;
using System.Collections.Generic;
using CallGate.ApiModels.User;
using CallGate.DependencyInjection;

namespace CallGate.Services.Channel
{
    public interface IChannelUserService : ITransientDependency
    {
        void AddAuthorizedUserToChannel(Guid channelId);
        
        void RemoveAuthorizedUserFromChannel(Guid channelId);
        
        void AddUserToChannelByUsername(string username, Guid channelId);
        
        void RemoveUserFromChannelByUsername(string username, Guid channelId);
        
        IEnumerable<UserResponse> GetAllChannelUsers(Guid channelId, string username);
        
        IEnumerable<UserResponse> GetGroupUsersOutsideChannel(string username, Guid channelId);
    }
}