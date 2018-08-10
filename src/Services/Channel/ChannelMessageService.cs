using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CallGate.ApiModels.ChannelMessage;
using CallGate.Stores;

namespace CallGate.Services.Channel
{
    public class ChannelMessageService : IChannelMessageService
    {
        private readonly IChannelUserStore _channelUserStore;
        private readonly IMessageStore _messageStore;
        private readonly IMapper _mapper;

        public ChannelMessageService(
            IChannelUserStore channelUserStore,
            IMessageStore messageStore,
            IMapper mapper
        ){
            _channelUserStore = channelUserStore;
            _messageStore = messageStore;
            _mapper = mapper;
        }

        public IEnumerable<ChannelMessageResponse> GetAllByChannelId(Guid channelId)
        {
            var channelMessages = _messageStore.GetAllChannelMessagesByChannelId(channelId);
            
            return _mapper.Map<IEnumerable<ChannelMessageResponse>>(channelMessages);
        }

        public Task<Documents.Message> AddMessageToChannelAsync(Guid channelId, string content, Models.User user)
        {
            RequireUserIsChannelMember(user.Id, channelId);
            
            var message = new Documents.Message
            {
                Id = Guid.NewGuid(),
                Content = content,
                Created = DateTime.Now,
                ChannelId = channelId,
                UserId = user.Id,
                Username = user.Username
            };
            
            _messageStore.Add(message);

            return Task.FromResult(message);
        }
        
        private void RequireUserIsChannelMember(Guid userId, Guid channelId)
        {
            var channelUser = _channelUserStore.GetOneByUserIdAndChannelId(userId, channelId);

            if (channelUser == null)
            {
                throw new Exception("You are not a member of this channel.");
            }
        }
    }
}