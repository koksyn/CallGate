using System;
using System.Collections.Generic;
using CallGate.Repositories;
using AutoMapper;
using CallGate.ApiModels.User;
using CallGate.Models;
using CallGate.Services.Helper;
using CallGate.Stores;

namespace CallGate.Services.Chat
{
    public class ChatUserService : IChatUserService
    {
        private readonly IGroupUserRepository _groupUserRepository;
        private readonly IChatUserRepository _chatUserRepository;
        private readonly IChatEventService _chatEventService;
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IChatUserStore _chatUserStore;
        private readonly IMapper _mapper;
        private readonly Models.User _authorizedUser;

        public ChatUserService(
            IAuthorizedUserHelper authorizedUserHelper, 
            IGroupUserRepository groupUserRepository,
            IChatUserRepository chatUserRepository,
            IChatEventService chatEventService,
            IChatRepository chatRepository,
            IUserRepository userRepository,
            IChatUserStore chatUserStore,
            IMapper mapper
        )
        {
            _groupUserRepository = groupUserRepository;
            _chatUserRepository = chatUserRepository;
            _chatEventService = chatEventService;
            _chatRepository = chatRepository;
            _userRepository = userRepository;
            _chatUserStore = chatUserStore;
            _mapper = mapper;
            _authorizedUser = authorizedUserHelper.GetAuthorizedUser();
        }
        
        public void AddUserToChatByUsername(string username, Guid chatId)
        {
            var user = _userRepository.GetUserByUsername(username);
            
            var chat = _chatRepository.Get(chatId);
            var chatUser = new ChatUser(user, chat);
            
            _chatUserRepository.Add(chatUser);

            var chatUserDocument = new Documents.ChatUser(user.Id, chatId);
            _chatUserStore.AddToBus(chatUserDocument);
            
            _chatEventService.AddUserAddedToChatEvent(chat.Group, _authorizedUser, user, chat);
        }

        public IEnumerable<UserResponse> GetAllChatUsers(Guid chatId, string username)
        {
            var users = _userRepository.GetAllUsersByChatIdAndUsername(chatId, username);

            return _mapper.Map<IEnumerable<UserResponse>>(users);
        }
        
        public IEnumerable<UserResponse> GetGroupUsersOutsideChat(string username, Guid chatId)
        {
            var chat = _chatRepository.Get(chatId);
            var groupUsers = _groupUserRepository.GetAllByUsernameAndGroupIdOutsideChatId(username, chat.GroupId, chat.Id);

            return _mapper.Map<IEnumerable<UserResponse>>(groupUsers);
        }
    }
}