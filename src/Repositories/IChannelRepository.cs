using System;
using System.Collections.Generic;
using CallGate.DependencyInjection;
using CallGate.Models;

namespace CallGate.Repositories
{
    public interface IChannelRepository : IRepository<Channel>, ITransientDependency
    {
        IEnumerable<Channel> GetAllByGroupId(Guid groupId);
        
        IEnumerable<Channel> GetAllByNameAndGroupIdConnectedWithUserId(Guid groupId, Guid userId, string channelName);
        
        IEnumerable<Channel> GetAllByNameAndGroupIdNotConnectedWithUserId(Guid groupId, Guid userId, string channelName);
    }
}