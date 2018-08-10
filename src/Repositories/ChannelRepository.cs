using System;
using System.Collections.Generic;
using System.Linq;
using CallGate.Data;
using CallGate.Models;
using Microsoft.EntityFrameworkCore;

namespace CallGate.Repositories
{
    public class ChannelRepository : Repository<Channel>, IChannelRepository
    {
        public ChannelRepository(DatabaseContext databaseContext) : base(databaseContext) {}

        public IEnumerable<Channel> GetAllByGroupId(Guid groupId)
        {
            return DbSet
                .Where(channel => channel.GroupId == groupId)
                .ToList();
        }

        public IEnumerable<Channel> GetAllByNameAndGroupIdConnectedWithUserId(Guid groupId, Guid userId, string channelName)
        {
            return DbSet
                .Where(channel => (channel.GroupId == groupId) && ContainsWhenNotEmpty(channel.Name, channelName))
                .Include(c => c.ChannelUsers)
                .ThenInclude(cu => cu.User)
                .Where(c => c.ChannelUsers.Any(cu => cu.UserId == userId))
                .ToList();
        }

        public IEnumerable<Channel> GetAllByNameAndGroupIdNotConnectedWithUserId(Guid groupId, Guid userId, string channelName)
        {
            return DbSet
                .Where(channel => (channel.GroupId == groupId) && ContainsWhenNotEmpty(channel.Name, channelName))
                .Include(c => c.ChannelUsers)
                .ThenInclude(cu => cu.User)
                .Where(c => c.ChannelUsers.All(cu => cu.UserId != userId))
                .ToList();
        }
    }
}
