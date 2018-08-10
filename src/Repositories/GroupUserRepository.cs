using System;
using System.Collections.Generic;
using CallGate.Data;
using CallGate.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CallGate.Repositories
{
    public class GroupUserRepository : Repository<GroupUser>, IGroupUserRepository
    {
        public GroupUserRepository(DatabaseContext databaseContext) : base(databaseContext) {}
        
        public GroupUser GetByUserIdAndGroupId(Guid userId, Guid groupId)
        {
            return DbContext.Users
                .Where(u => u.Id == userId)
                .SelectMany(gu => gu.GroupUsers)
                .Include(gu => gu.Group)
                .SingleOrDefault(gu => gu.GroupId == groupId);
        }

        public GroupUser GetByUserIdAndGroupIdAndRole(Guid userId, Guid groupId, Role role)
        {
            return DbContext.Users
                .Where(u => u.Id == userId)
                .SelectMany(gu => gu.GroupUsers)
                .Include(gu => gu.Group)
                .Where(gu => gu.Role == role)
                .SingleOrDefault(gu => gu.GroupId == groupId);
        }

        public IEnumerable<GroupUser> GetAllByUsernameAndGroupId(string username, Guid groupId)
        {
            return DbContext.Users
                .Where(u => ContainsWhenNotEmpty(u.Username, username))
                .SelectMany(gu => gu.GroupUsers)
                .Where(gu => gu.GroupId == groupId)
                .Include(gu => gu.User)
                .Distinct()
                .ToList();
        }

        public IEnumerable<GroupUser> GetAllByUsernameAndGroupIdOutsideChatId(string username, Guid groupId, Guid chatId)
        {
            return DbContext.Users
                .Where(u => ContainsWhenNotEmpty(u.Username, username))
                .Where(u => u.ChatUsers.All(cu => cu.ChatId != chatId))
                .SelectMany(gu => gu.GroupUsers)
                .Where(gu => gu.GroupId == groupId)
                .Include(gu => gu.User)
                .Distinct()
                .ToList();
        }

        public IEnumerable<GroupUser> GetAllByUsernameAndGroupIdOutsideChannelId(string username, Guid groupId, Guid channelId)
        {
            return DbContext.Users
                .Where(u => ContainsWhenNotEmpty(u.Username, username))
                .Where(u => u.ChannelUsers.All(cu => cu.ChannelId != channelId))
                .SelectMany(gu => gu.GroupUsers)
                .Where(gu => gu.GroupId == groupId)
                .Include(gu => gu.User)
                .Distinct()
                .ToList();
        }
    }
}