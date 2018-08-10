using System;
using System.Collections.Generic;
using CallGate.Repositories;
using AutoMapper;
using CallGate.ApiModels.User;
using CallGate.Models;
using CallGate.Services.Email;
using CallGate.Services.Helper;
using CallGate.Stores;

namespace CallGate.Services.Channel
{
    public class ChannelUserService : IChannelUserService
    {
        private readonly IGroupUserRepository _groupUserRepository;
        private readonly IChannelUserRepository _channelUserRepository;
        private readonly IChannelEventService _channelEventService;
        private readonly IChannelRepository _channelRepository;
        private readonly IUserRepository _userRepository;
        private readonly IChannelUserStore _channelUserStore;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        private readonly Models.User _authorizedUser;

        public ChannelUserService(
            IAuthorizedUserHelper authorizedUserHelper,
            IGroupUserRepository groupUserRepository,
            IChannelUserRepository channelUserRepository,
            IChannelEventService channelEventService,
            IChannelRepository channelRepository,
            IUserRepository userRepository,
            IChannelUserStore channelUserStore,
            IMailService mailService,
            IMapper mapper
        )
        {
            _groupUserRepository = groupUserRepository;
            _channelUserRepository = channelUserRepository;
            _channelEventService = channelEventService;
            _channelRepository = channelRepository;
            _userRepository = userRepository;
            _channelUserStore = channelUserStore;
            _mailService = mailService;
            _mapper = mapper;
            _authorizedUser = authorizedUserHelper.GetAuthorizedUser();
        }

        public void AddAuthorizedUserToChannel(Guid channelId)
        {
            AddUserToChannel(_authorizedUser, channelId);
        }
        
        public void RemoveAuthorizedUserFromChannel(Guid channelId)
        {
            RemoveUserFromChannel(_authorizedUser, channelId);
        }

        public void AddUserToChannelByUsername(string username, Guid channelId)
        {
            var user = _userRepository.GetUserByUsername(username);
            
            AddUserToChannel(user, channelId);
        }
        
        public void RemoveUserFromChannelByUsername(string username, Guid channelId)
        {
            var user = _userRepository.GetUserByUsername(username);
            
            RemoveUserFromChannel(user, channelId);

            var channel = _channelRepository.Get(channelId);
            
            _mailService.SendMail(
                user.Email, 
                $"CallGate - Group administrator removed you from channel '{channel.Name}'", 
                $"Group administrator '{_authorizedUser.Username}' removed you from channel '{channel.Name}'" +
                $" in group '{channel.Group.Name}'" 
            );
        }
        
        public IEnumerable<UserResponse> GetAllChannelUsers(Guid channelId, string username)
        {
            var users = _userRepository.GetAllUsersByChannelIdAndUsername(channelId, username);

            return _mapper.Map<IEnumerable<UserResponse>>(users);
        }
        
        public IEnumerable<UserResponse> GetGroupUsersOutsideChannel(string username, Guid channelId)
        {
            var channel = _channelRepository.Get(channelId);
            var groupUsers = _groupUserRepository.GetAllByUsernameAndGroupIdOutsideChannelId(username, channel.GroupId, channel.Id);

            return _mapper.Map<IEnumerable<UserResponse>>(groupUsers);
        }
        
        private void AddUserToChannel(Models.User user, Guid channelId)
        {
            var channel = _channelRepository.Get(channelId);
            
            var channelUser = new ChannelUser(user, channel);
            _channelUserRepository.Add(channelUser);
            
            var channelUserDocument = new Documents.ChannelUser(user.Id, channelId);
            _channelUserStore.AddToBus(channelUserDocument);
            
            _channelEventService.AddUserAddedToChannelEvent(channel.Group, _authorizedUser, user, channel);
        }
        
        private void RemoveUserFromChannel(Models.User user, Guid channelId)
        {
            var channel = _channelRepository.Get(channelId);
            var channelUser = _channelUserRepository.GetByUserIdAndChannelId(user.Id, channelId);
            
            _channelUserRepository.Remove(channelUser);
            _channelUserStore.RemoveByUserIdAndChannelId(user.Id, channelId);
            _channelEventService.AddUserRemovedFromChannelEvent(channel.Group, _authorizedUser, user, channel);
        }
    }
}