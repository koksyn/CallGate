using System;
using CallGate.DependencyInjection;
using CallGate.Documents;

namespace CallGate.Stores
{
    public interface IChatUserStore : IStore<ChatUser>, IScopedDependency
    {
        ChatUser GetOneByUserIdAndChatId(Guid userId, Guid chatId);
        
        void RemoveByUserIdAndChatId(Guid userId, Guid chatId);
        
        void RemoveAllByChatId(Guid chatId);
    }
}