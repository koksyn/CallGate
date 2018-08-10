using System;
using System.Collections.Generic;
using CallGate.ApiModels.Channel;
using CallGate.DependencyInjection;

namespace CallGate.Services.Channel
{
    public interface IChannelService : ITransientDependency
    {
        ChannelResponse Get(Guid channelId);
        
        IEnumerable<ChannelResponse> GetChannelsForGroup(Guid groupId);
        
        IEnumerable<ChannelResponse> GetChannelsInGroupConnectedWithLoggedUser(Guid groupId, string channelName);
        
        IEnumerable<ChannelResponse> GetChannelsInGroupNotConnectedWithLoggedUser(Guid groupId, string channelName);
        
        ChannelResponse Create(Guid groupId, string name);
    }
}