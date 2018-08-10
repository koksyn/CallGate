using System;
using System.Collections.Generic;
using System.Linq;
using CallGate.Repositories;
using AutoMapper;
using CallGate.ApiModels.User;
using CallGate.Models;
using CallGate.Services.Channel;
using CallGate.Services.Chat;
using CallGate.Services.Email;
using CallGate.Services.Helper;
using CallGate.Stores;

namespace CallGate.Services.Group
{
    public class GroupUserService : IGroupUserService
    {
        private readonly IGroupUserRepository _groupUserRepository;
        private readonly IGroupUserStore _groupUserStore;
        private readonly IGroupEventService _groupEventService;
        private readonly IGroupRepository _groupRepository;
        private readonly IChannelEventService _channelEventService;
        private readonly IChannelUserRepository _channelUserRepository;
        private readonly IChannelUserStore _channelUserStore;
        private readonly IChatEventService _chatEventService;
        private readonly IChatUserRepository _chatUserRepository;
        private readonly IChatUserStore _chatUserStore;
        private readonly IChatService _chatService;
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        private readonly Models.User _authorizedUser;

        public GroupUserService(
            IAuthorizedUserHelper authorizedUserHelper,
            IGroupUserRepository groupUserRepository,
            IGroupUserStore groupUserStore,
            IGroupEventService groupEventService,
            IGroupRepository groupRepository,
            IChannelEventService channelEventService,
            IChannelUserRepository channelUserRepository,
            IChannelUserStore channelUserStore,
            IChatEventService chatEventService,
            IChatUserRepository chatUserRepository,
            IChatUserStore chatUserStore,
            IChatService chatService,
            IUserRepository userRepository,
            IMailService mailService,
            IMapper mapper
        )
        {
            _groupUserRepository = groupUserRepository;
            _groupUserStore = groupUserStore;
            _groupEventService = groupEventService;
            _groupRepository = groupRepository;
            _channelEventService = channelEventService;
            _channelUserRepository = channelUserRepository;
            _channelUserStore = channelUserStore;
            _chatUserRepository = chatUserRepository;
            _chatUserStore = chatUserStore;
            _chatEventService = chatEventService;
            _chatService = chatService;
            _userRepository = userRepository;
            _mailService = mailService;
            _mapper = mapper;
            _authorizedUser = authorizedUserHelper.GetAuthorizedUser();
        }
        
        public void AddUserToGroup(string username, Role role, Guid groupId)
        {
            var user = _userRepository.GetUserByUsername(username);
            var group = _groupRepository.Get(groupId);
            
            var groupUser = new GroupUser(user, group, role);
            var documentGroupUser = new Documents.GroupUser(user.Id, group.Id);
            
            _groupUserRepository.Add(groupUser);
            _groupUserStore.AddToBus(documentGroupUser);
            _groupEventService.AddUserAddedToGroupEvent(group, _authorizedUser, user);
            
            _mailService.SendMail(
                user.Email, 
                $"CallGate - Welcome in the group '{group.Name}'", 
                $"Group member '{_authorizedUser.Username}' added you to the group '{group.Name}'. Welcome!"
            );
        }

        public void EditGroupUser(string username, Role role, Guid groupId)
        {
            var user = _userRepository.GetUserByUsername(username);
            var groupUser = _groupUserRepository.GetByUserIdAndGroupId(user.Id, groupId);

            groupUser.Role = role;

            _groupEventService.AddRoleInGroupGrantedEvent(groupUser.Group, _authorizedUser, user, role);
            
            var roleName = Enum.GetName(typeof(Role), role);
            
            _mailService.SendMail(
                user.Email, 
                $"CallGate - Your privileges in group '{groupUser.Group.Name}' have been changed.", 
                $"Group administrator '{_authorizedUser.Username}' changed your privileges in '{groupUser.Group.Name}'" +
                $" to '{roleName}'" 
            );
        }

        public void RemoveAuthorizedUserFromGroup(Guid groupId)
        {
            RemoveUserFromGroup(_authorizedUser, groupId);
        }

        public void RemoveAuthorizedUserFromAssociatedGroups()
        {
            IEnumerable<Models.Group> groups = _groupRepository.GetAllByUserId(_authorizedUser.Id);

            if (groups == null || !groups.Any()) return;
        
            foreach (var group in groups)
            {
                RemoveUserFromGroup(_authorizedUser, group.Id);
            }
        }

        public void RemoveUserFromGroupByUsername(string username, Guid groupId)
        {
            var user = _userRepository.GetUserByUsername(username);
            
            RemoveUserFromGroup(user, groupId);
            
            var group = _groupRepository.Get(groupId);
            
            _mailService.SendMail(
                user.Email, 
                $"CallGate - Group administrator removed you from group '{group.Name}'", 
                $"Group administrator '{_authorizedUser.Username}' removed you from group '{group.Name}'"
            );
        }

        public IEnumerable<UserResponse> GetAllGroupMembers(Guid groupId, string username)
        {
            var users = _userRepository.GetAllUsersByGroupIdAndUsername(groupId, username);

            return _mapper.Map<IEnumerable<UserResponse>>(users);
        }
        
        public IEnumerable<UserResponse> GetAllUsersOutsideGroup(Guid groupId, string username)
        {
            var exceptUsers = _userRepository.GetAllUsersByGroupIdAndUsername(groupId, username);
            var users = _userRepository.GetAllByUsernameExceptEnumerable(username, exceptUsers);

            return _mapper.Map<IEnumerable<UserResponse>>(users);
        }
        
        public IEnumerable<UserDetailsResponse> GetAllGroupMembersDetails(Guid groupId, string username)
        {
            var groupUsers = _groupUserRepository.GetAllByUsernameAndGroupId(username, groupId);

            return _mapper.Map<IEnumerable<UserDetailsResponse>>(groupUsers);
        }
        
        public IEnumerable<UserResponse> GetAllUsersOutsideConnectedChats(Guid groupId, int chatUsersCount)
        {
            var usersFromConnectedChats = _userRepository.GetAllFromChatsConnectedWithUserIdByGroupIdAndUsersCount(
                _authorizedUser.Id, 
                groupId,
                chatUsersCount
            );
            
            var users = _userRepository.GetAllByGroupIdExceptEnumerableAndUserId(groupId, usersFromConnectedChats, _authorizedUser.Id);
            
            return _mapper.Map<IEnumerable<UserResponse>>(users);
        }

        public string GetAuthorizedUserRoleNameInGroup(Guid groupId)
        {
            var groupUser = _groupUserRepository.GetByUserIdAndGroupId(_authorizedUser.Id, groupId);
            
            return Enum.GetName(typeof(Role), groupUser.Role);
        }

        public void RemoveUserFromGroup(Models.User user, Guid groupId)
        {
            RemoveUserFromAssociatedChatsInGroup(user, groupId);
            RemoveUserFromAssociatedChannelsInGroup(user, groupId);
            
            var groupUser = _groupUserRepository.GetByUserIdAndGroupId(user.Id, groupId);
            
            _groupUserRepository.Remove(groupUser);
            _groupUserStore.RemoveByUserIdAndGroupId(user.Id, groupId);
            
            var group = _groupRepository.Get(groupId);
            _groupEventService.AddUserRemovedFromGroupEvent(group, _authorizedUser, user);
        }

        private void RemoveUserFromAssociatedChatsInGroup(Models.User user, Guid groupId)
        {
            IEnumerable<ChatUser> chatUsers = _chatUserRepository.GetAllByUserIdAndGroupId(user.Id, groupId);

            if (chatUsers == null || !chatUsers.Any()) return;
            
            var group = _groupRepository.Get(groupId);
            
            foreach (var chatUser in chatUsers)
            {
                var chatId = chatUser.ChatId;
                var usersInChat = _chatUserRepository.CountByChatId(chatId);
                
                if (usersInChat <= 3)
                {
                    _chatService.RemoveChatByChatId(chatId);
                }
                else // remove only one chatUser
                {
                    _chatUserRepository.Remove(chatUser);
                    _chatUserStore.RemoveByUserIdAndChatId(user.Id, chatId);
                    _chatEventService.AddUserRemovedFromChatEvent(group, _authorizedUser, user, chatUser.Chat);
                }
            }
        }
        
        private void RemoveUserFromAssociatedChannelsInGroup(Models.User user, Guid groupId)
        {
            IEnumerable<ChannelUser> channelUsers = _channelUserRepository.GetAllByUserIdAndGroupId(user.Id, groupId);

            if (channelUsers == null || !channelUsers.Any()) return;
            
            var group = _groupRepository.Get(groupId);
            
            foreach (var channelUser in channelUsers)
            {
                _channelUserRepository.Remove(channelUser);
                _channelUserStore.RemoveByUserIdAndChannelId(user.Id, channelUser.ChannelId);
                _channelEventService.AddUserRemovedFromChannelEvent(group, _authorizedUser, user, channelUser.Channel);
            }
        }
    }
}