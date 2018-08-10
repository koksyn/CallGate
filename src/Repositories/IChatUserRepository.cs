using System;
using System.Collections;
using System.Collections.Generic;
using CallGate.DependencyInjection;
using CallGate.Models;

namespace CallGate.Repositories
{
    public interface IChatUserRepository : IRepository<ChatUser>, ITransientDependency
    {
        ChatUser GetByUserIdAndChatId(Guid userId, Guid chatId);
        
        IEnumerable<ChatUser> GetAllByChatId(Guid chatId);
        
        IEnumerable<ChatUser> GetAllByUserIdAndGroupId(Guid userId, Guid groupId);
        
        int CountByChatId(Guid chatId);
        
        void RemoveAllByChatId(Guid chatId);
    }
}