using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CallGate.ApiModels.ChannelMessage;
using CallGate.DependencyInjection;

namespace CallGate.Services.Channel
{
    public interface IChannelMessageService : ITransientDependency
    {
        IEnumerable<ChannelMessageResponse> GetAllByChannelId(Guid channelId);
        
        Task<Documents.Message> AddMessageToChannelAsync(Guid channelId, string content, Models.User user);
    }
}