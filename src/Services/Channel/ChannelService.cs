using System;
using System.Collections.Generic;
using AutoMapper;
using CallGate.ApiModels.Channel;
using CallGate.Models;
using CallGate.Repositories;
using CallGate.Services.Helper;
using CallGate.Stores;

namespace CallGate.Services.Channel
{
    public class ChannelService : IChannelService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IChannelUserRepository _channelUserRepository;
        private readonly IChannelEventService _channelEventService;
        private readonly IChannelUserStore _channelUserStore;
        private readonly IChannelRepository _channelRepository;
        private readonly IMapper _mapper;
        private readonly Models.User _authorizedUser;

        public ChannelService(
            IAuthorizedUserHelper authorizedUserHelper, 
            IGroupRepository groupRepository,
            IChannelUserRepository channelUserRepository,
            IChannelEventService channelEventService,
            IChannelUserStore channelUserStore,
            IChannelRepository channelRepository,
            IMapper mapper
        ){
            _groupRepository = groupRepository;
            _channelUserRepository = channelUserRepository;
            _channelEventService = channelEventService;
            _authorizedUser = authorizedUserHelper.GetAuthorizedUser();
            _channelUserStore = channelUserStore;
            _channelRepository = channelRepository;
            _mapper = mapper;
        }

        public ChannelResponse Get(Guid channelId)
        {
            var channel = _channelRepository.Get(channelId);
            
            return _mapper.Map<ChannelResponse>(channel);
        }

        public IEnumerable<ChannelResponse> GetChannelsForGroup(Guid groupId)
        {
            var channels = _channelRepository.GetAllByGroupId(groupId);

            return _mapper.Map<IEnumerable<ChannelResponse>>(channels);
        }

        public IEnumerable<ChannelResponse> GetChannelsInGroupConnectedWithLoggedUser(Guid groupId, string channelName)
        {
            var channels = _channelRepository.GetAllByNameAndGroupIdConnectedWithUserId(groupId, _authorizedUser.Id, channelName);

            return _mapper.Map<IEnumerable<ChannelResponse>>(channels);
        }

        public IEnumerable<ChannelResponse> GetChannelsInGroupNotConnectedWithLoggedUser(Guid groupId, string channelName)
        {
            var channels = _channelRepository.GetAllByNameAndGroupIdNotConnectedWithUserId(groupId, _authorizedUser.Id, channelName);

            return _mapper.Map<IEnumerable<ChannelResponse>>(channels);
        }

        public ChannelResponse Create(Guid groupId, string name)
        {
            var channel = new Models.Channel
            {
                Id = Guid.NewGuid(),
                GroupId = groupId,
                Name = name
            };
                
            var channelUser = new ChannelUser(_authorizedUser, channel);
            _channelUserRepository.Add(channelUser);
            
            var channelUserDocument = new Documents.ChannelUser(_authorizedUser.Id, channel.Id);
            _channelUserStore.AddToBus(channelUserDocument);

            var group = _groupRepository.Get(groupId);
            _channelEventService.AddChannelCreatedEvent(group, _authorizedUser, channel);
            _channelEventService.AddUserAddedToChannelEvent(group, _authorizedUser, _authorizedUser, channel);
            
            return _mapper.Map<ChannelResponse>(channel);
        }
    }
}