using System;
using System.Collections.Generic;
using CallGate.ApiModels.Chat;
using CallGate.DependencyInjection;

namespace CallGate.Services.Chat
{
    public interface IChatService : ITransientDependency
    {
        IEnumerable<ChatResponse> GetGroupChatsForLoggedUser(Guid groupId);
        
        ChatResponse Create(Guid groupId, string username);
        
        int GetGroupChatsCount(Guid groupId);
        
        void RemoveChatByChatId(Guid chatId);
    }
}