using System;
using System.Collections.Generic;
using CallGate.Data;
using CallGate.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CallGate.Repositories
{
    public class ChannelUserRepository : Repository<ChannelUser>, IChannelUserRepository
    {
        public ChannelUserRepository(DatabaseContext databaseContext) : base(databaseContext) {}
        
        public ChannelUser GetByUserIdAndChannelId(Guid userId, Guid channelId)
        {
            return DbContext.Users
                .Where(u => u.Id == userId)
                .SelectMany(cu => cu.ChannelUsers)
                .Include(cu => cu.Channel)
                .SingleOrDefault(cu => cu.ChannelId == channelId);
        }

        public IEnumerable<ChannelUser> GetAllByUserIdAndGroupId(Guid userId, Guid groupId)
        {
            return DbContext.Users
                .Where(u => u.Id == userId)
                .SelectMany(cu => cu.ChannelUsers)
                .Include(cu => cu.Channel)
                .Where(cu => cu.Channel.GroupId == groupId)
                .ToList();
        }
    }
}