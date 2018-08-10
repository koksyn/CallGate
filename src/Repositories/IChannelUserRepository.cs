using System;
using System.Collections.Generic;
using CallGate.DependencyInjection;
using CallGate.Models;

namespace CallGate.Repositories
{
    public interface IChannelUserRepository : IRepository<ChannelUser>, ITransientDependency
    {
        ChannelUser GetByUserIdAndChannelId(Guid userId, Guid channelId);
        
        IEnumerable<ChannelUser> GetAllByUserIdAndGroupId(Guid userId, Guid groupId);
    }
}