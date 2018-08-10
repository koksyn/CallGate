using System;
using CallGate.DependencyInjection;
using CallGate.Documents;

namespace CallGate.Stores
{
    public interface IChannelUserStore : IStore<ChannelUser>, IScopedDependency
    {
        ChannelUser GetOneByUserIdAndChannelId(Guid userId, Guid channelId);
        
        void RemoveByUserIdAndChannelId(Guid userId, Guid channelId);
    }
}