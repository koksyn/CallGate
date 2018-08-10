using System;
using System.Collections.Generic;
using System.Linq;
using CallGate.Data;
using CallGate.Models;
using Microsoft.EntityFrameworkCore;

namespace CallGate.Repositories
{
    public class ChatRepository : Repository<Chat>, IChatRepository
    {
        public ChatRepository(DatabaseContext databaseContext) : base(databaseContext) {}

        public IEnumerable<Chat> GetAllByUserIdAndGroupId(Guid userId, Guid groupId)
        {
            return DbContext.Chats
                .Where(c => c.GroupId == groupId)
                .Include(c => c.ChatUsers)
                .ThenInclude(cu => cu.User)
                .Where(c => c.ChatUsers.Any(cu => cu.UserId == userId))
                .ToList();
        }
        
        public Chat GetChatWithOnlyTwoUsersByGroupId(Guid userId, Guid secondUserId, Guid groupId)
        {
            return DbContext.Chats
                .Where(c => c.GroupId == groupId)
                .Include(c => c.ChatUsers)
                .ThenInclude(cu => cu.User)
                .Where(c => c.ChatUsers.Count == 2)
                .Where(c => c.ChatUsers.Any(cu => cu.UserId == userId))
                .SingleOrDefault(c => c.ChatUsers.Any(cu => cu.UserId == secondUserId));
        }

        public int CountByGroupId(Guid groupId)
        {
            return DbContext.Chats
                .Count(c => c.GroupId == groupId);
        }
    }
}