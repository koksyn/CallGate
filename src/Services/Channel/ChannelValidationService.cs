using System;
using CallGate.Exceptions;
using CallGate.Repositories;
using CallGate.Services.Group;
using CallGate.Services.Helper;
using CallGate.Services.User;

namespace CallGate.Services.Channel
{
    public class ChannelValidationService : IChannelValidationService
    {
        private readonly IChannelUserRepository _channelUserRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IUserValidationService _userValidationService;
        private readonly IGroupValidationService _groupValidationService;
        private readonly Models.User _authorizedUser;

        public ChannelValidationService(
            IAuthorizedUserHelper authorizedUserHelper,
            IChannelUserRepository channelUserRepository,
            IChannelRepository channelRepository,
            IUserValidationService userValidationService,
            IGroupValidationService groupValidationService
        )
        {
            _channelUserRepository = channelUserRepository;
            _channelRepository = channelRepository;
            _userValidationService = userValidationService;
            _groupValidationService = groupValidationService;
            _authorizedUser = authorizedUserHelper.GetAuthorizedUser();
        }

        public void RequireUserIsChannelMember(string username, Guid channelId)
        {
            var user = _userValidationService.RequireAndGetUserByUsername(username);
            
            RequireUserByIdIsChannelMember(
                user.Id, 
                channelId, 
                "User is not a member of this channel."
            );
        }

        public void RequireAuthorizedUserIsChannelMember(Guid channelId)
        {
            RequireUserByIdIsChannelMember(
                _authorizedUser.Id, 
                channelId, 
                "You are not a member of this channel."
            );
        }

        public void RequireUserIsNotChannelMember(string username, Guid channelId)
        {
            var user = _userValidationService.RequireAndGetUserByUsername(username);
            
            RequireUserByIdIsNotChannelMember(
                user.Id, 
                channelId, 
                "User is already member of this channel."
            );
        }
        
        public void RequireAuthorizedUserIsNotChannelMember(Guid channelId)
        {
            RequireUserByIdIsNotChannelMember(
                _authorizedUser.Id, 
                channelId, 
                "You are already member of this channel."
            );
        }

        public void RequireUserIsGroupMemberFromChannel(string username, Guid channelId)
        {
            var user = _userValidationService.RequireAndGetUserByUsername(username);
            var channel = RequireAndGetChannelById(channelId);

            _groupValidationService.RequireUserIdIsGroupMember(
                user.Id, 
                channel.GroupId, 
                "User is not a member of group, to which this channel belongs."
            );
        }
        
        public void RequireAuthorizedUserIsGroupMemberFromChannel(Guid channelId)
        {
            var channel = RequireAndGetChannelById(channelId);

            _groupValidationService.RequireUserIdIsGroupMember(
                _authorizedUser.Id, 
                channel.GroupId, 
                "You are not a member of group, to which this channel belongs."
            );
        }

        public void RequireAuthorizedUserIsGroupAdminFromChannel(Guid channelId)
        {
            var channel = RequireAndGetChannelById(channelId);
            
            _groupValidationService.RequireUserIdIsGroupAdmin(
                _authorizedUser.Id, 
                channel.GroupId,
                "You are not a administrator of group, to which this channel belongs."
            );
        }
        
        private Models.Channel RequireAndGetChannelById(Guid channelId)
        {
            var channel = _channelRepository.Get(channelId);
            
            if (channel == null)
            {
                throw new ResourceNotFoundApiException(channelId.ToString(), "Channel");
            }

            return channel;
        }

        private void RequireUserByIdIsChannelMember(Guid userId, Guid channelId, string errorMessage)
        {
            var channelUser = _channelUserRepository.GetByUserIdAndChannelId(userId, channelId);

            if (channelUser == null)
            {
                throw new LogicApiException(errorMessage);
            }
        }
        
        private void RequireUserByIdIsNotChannelMember(Guid userId, Guid channelId, string errorMessage)
        {
            var channelUser = _channelUserRepository.GetByUserIdAndChannelId(userId, channelId);

            if (channelUser != null)
            {
                throw new LogicApiException(errorMessage);
            }
        }
    }
}