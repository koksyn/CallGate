using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CallGate.ApiModels.Chat;
using CallGate.Models;
using CallGate.Repositories;
using CallGate.Services.Helper;
using CallGate.Stores;

namespace CallGate.Services.Chat
{
    public class ChatService : IChatService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IChatUserRepository _chatUserRepository;
        private readonly IChatEventService _chatEventService;
        private readonly IChatUserStore _chatUserStore;
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly Models.User _authorizedUser;

        public ChatService(
            IAuthorizedUserHelper authorizedUserHelper, 
            IGroupRepository groupRepository,
            IChatUserRepository chatUserRepository,
            IChatEventService chatEventService,
            IChatUserStore chatUserStore,
            IChatRepository chatRepository, 
            IUserRepository userRepository,
            IMapper mapper
        ){
            _groupRepository = groupRepository;
            _chatUserRepository = chatUserRepository;
            _chatEventService = chatEventService;
            _chatUserStore = chatUserStore;
            _chatRepository = chatRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _authorizedUser = authorizedUserHelper.GetAuthorizedUser();
        }

        public IEnumerable<ChatResponse> GetGroupChatsForLoggedUser(Guid groupId)
        {
            var chats = _chatRepository
                .GetAllByUserIdAndGroupId(_authorizedUser.Id, groupId)
                .ToList();

            foreach (var chat in chats)
            {
                var chatUsersWithoutLoggedUser = chat.ChatUsers
                    .Where(cu => cu.UserId != _authorizedUser.Id)
                    .ToList();

                chat.ChatUsers = chatUsersWithoutLoggedUser;
            }
            
            return _mapper.Map<IEnumerable<ChatResponse>>(chats);
        }

        public ChatResponse Create(Guid groupId, string username)
        {
            var chat = new Models.Chat
            {
                Id = Guid.NewGuid(),
                GroupId = groupId
            };
            
            var chatUserAuthorized = new ChatUser(_authorizedUser, chat);

            _chatUserRepository.Add(chatUserAuthorized);
            
            var user = _userRepository.GetUserByUsername(username);
            var chatUser = new ChatUser(user, chat);
            
            _chatUserRepository.Add(chatUser);
            
            var chatUserAuthorizedDocument = new Documents.ChatUser(_authorizedUser.Id, chat.Id);
            var chatUserDocument = new Documents.ChatUser(user.Id, chat.Id);
            
            _chatUserStore.AddToBus(chatUserAuthorizedDocument);
            _chatUserStore.AddToBus(chatUserDocument);

            var group = _groupRepository.Get(groupId);
            _chatEventService.AddChatCreatedEvent(group, _authorizedUser, chat);
            _chatEventService.AddUserAddedToChatEvent(group, _authorizedUser, _authorizedUser, chat);
            _chatEventService.AddUserAddedToChatEvent(group, _authorizedUser, user, chat);

            return _mapper.Map<ChatResponse>(chat);
        }

        public int GetGroupChatsCount(Guid groupId)
        {
            return _chatRepository.CountByGroupId(groupId);
        }

        public void RemoveChatByChatId(Guid chatId)
        {
            var chat = _chatRepository.Get(chatId);
            
            if (chat != null)
            {
                var group = _groupRepository.Get(chat.GroupId);
                
                RemoveAllChatUsers(chat, group);
                
                _chatRepository.Remove(chat);
                _chatEventService.AddChatRemovedEvent(group, _authorizedUser, chat);
            }
        }

        private void RemoveAllChatUsers(Models.Chat chat, Models.Group group)
        {
            var chatUsers = _chatUserRepository.GetAllByChatId(chat.Id);
                
            _chatUserRepository.RemoveAllByChatId(chat.Id);
            _chatUserStore.RemoveAllByChatId(chat.Id);

            foreach (var chatUser in chatUsers)
            {
                _chatEventService.AddUserRemovedFromChatEvent(group, _authorizedUser, chatUser.User, chat);
            }
        }
    }
}