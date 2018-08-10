using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CallGate.ApiModels.ChatMessage;
using CallGate.DependencyInjection;

namespace CallGate.Services.Chat
{
    public interface IChatMessageService : IScopedDependency
    {
        IEnumerable<ChatMessageResponse> GetAllByChatId(Guid chatId);
        
        Task<Documents.Message> AddMessageToChatAsync(Guid chatId, string content, Models.User user);
    }
}