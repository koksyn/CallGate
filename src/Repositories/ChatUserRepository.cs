using System;
using System.Collections.Generic;
using System.Linq;
using CallGate.Data;
using CallGate.Models;
using Microsoft.EntityFrameworkCore;

namespace CallGate.Repositories
{
    public class ChatUserRepository : Repository<ChatUser>, IChatUserRepository
    {
        public ChatUserRepository(DatabaseContext databaseContext) : base(databaseContext) {}
        
        public ChatUser GetByUserIdAndChatId(Guid userId, Guid chatId)
        {
            return DbContext.Users
                .Where(u => u.Id == userId)
                .SelectMany(cu => cu.ChatUsers)
                .Include(cu => cu.Chat)
                .SingleOrDefault(cu => cu.ChatId == chatId);
        }

        public IEnumerable<ChatUser> GetAllByChatId(Guid chatId)
        {
            return DbContext.Chats
                .Where(c => c.Id == chatId)
                .SelectMany(cu => cu.ChatUsers)
                .Include(cu => cu.Chat)
                .Include(cu => cu.User)
                .ToList();
        }

        public IEnumerable<ChatUser> GetAllByUserIdAndGroupId(Guid userId, Guid groupId)
        {
            return DbContext.Users
                .Where(u => u.Id == userId)
                .SelectMany(cu => cu.ChatUsers)
                .Include(cu => cu.Chat)
                .Where(cu => cu.Chat.GroupId == groupId)
                .ToList();
        }

        public int CountByChatId(Guid chatId)
        {
            return DbContext.Chats
                .Where(c => c.Id == chatId)
                .SelectMany(cu => cu.ChatUsers)
                .Count();
        }

        public void RemoveAllByChatId(Guid chatId)
        {
            var chatUsers = DbContext.Chats
                .Where(c => c.Id == chatId)
                .SelectMany(cu => cu.ChatUsers)
                .ToList();
            
            DbContext.RemoveRange(chatUsers);
        }
    }
}