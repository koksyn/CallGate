using System;
using System.Collections.Generic;
using CallGate.ApiModels.User;
using CallGate.DependencyInjection;

namespace CallGate.Services.Chat
{
    public interface IChatUserService : ITransientDependency
    {
        void AddUserToChatByUsername(string username, Guid chatId);
        
        IEnumerable<UserResponse> GetAllChatUsers(Guid chatId, string username);

        IEnumerable<UserResponse> GetGroupUsersOutsideChat(string username, Guid chatId);
    }
}