using System;
using CallGate.Repositories;
using CallGate.Exceptions;
using CallGate.Services.Group;
using CallGate.Services.Helper;
using CallGate.Services.User;

namespace CallGate.Services.Chat
{
    public class ChatValidationService : IChatValidationService
    {
        private readonly IChatUserRepository _chatUserRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IGroupValidationService _groupValidationService;
        private readonly IUserValidationService _userValidationService;
        private readonly Models.User _authorizedUser;

        public ChatValidationService(
            IAuthorizedUserHelper authorizedUserHelper,
            IChatUserRepository chatUserRepository,
            IChatRepository chatRepository,
            IGroupValidationService groupValidationService,
            IUserValidationService userValidationService
        )
        {
            _chatUserRepository = chatUserRepository;
            _groupValidationService = groupValidationService;
            _chatRepository = chatRepository;
            _userValidationService = userValidationService;
            _authorizedUser = authorizedUserHelper.GetAuthorizedUser();
        }
        
        public void RequireDifferentUsersForChatCreation(string username)
        {
            var user = _userValidationService.RequireAndGetUserByUsername(username);

            if (user.Id == _authorizedUser.Id)
            {
                throw new LogicApiException("You can not create a Chat with yourself");
            }
        }

        public void RequireChatBetweenUsersInGroupDoesNotExist(string username, Guid groupId)
        {
            var user = _userValidationService.RequireAndGetUserByUsername(username);
            
            var chat = _chatRepository.GetChatWithOnlyTwoUsersByGroupId(_authorizedUser.Id, user.Id, groupId);

            if (chat != null)
            {
                throw new LogicApiException("Private chat beetween you and this user already exist!");
            }
        }
        
        public void RequireUserIsNotChatMember(string username, Guid chatId)
        {
            var user = _userValidationService.RequireAndGetUserByUsername(username);
            
            var chatUser = _chatUserRepository.GetByUserIdAndChatId(user.Id, chatId);

            if (chatUser != null)
            {
                throw new LogicApiException("User is already member of this chat.");
            }
        }

        public void RequireAuthorizedUserIsChatMember(Guid chatId)
        {
            var chatUser = _chatUserRepository.GetByUserIdAndChatId(_authorizedUser.Id, chatId);

            if (chatUser == null)
            {
                throw new LogicApiException("You are not member of this Chat.");
            }
        }

        public void RequireUserIsGroupMemberFromChat(string username, Guid chatId)
        {
            var user = _userValidationService.RequireAndGetUserByUsername(username);
            var chat = RequireAndGetChatById(chatId);

            _groupValidationService.RequireUserIdIsGroupMember(
                user.Id, 
                chat.GroupId,
                "User is not a member of group, to which this chat belongs."
            );
        }

        public void RequireAuthorizedUserIsGroupMemberFromChat(Guid chatId)
        {
            var chat = RequireAndGetChatById(chatId);

            _groupValidationService.RequireUserIdIsGroupMember(
                _authorizedUser.Id, 
                chat.GroupId,
                "You are not a member of group, to which this chat belongs."
            );
        }
        
        private Models.Chat RequireAndGetChatById(Guid chatId)
        {
            var chat = _chatRepository.Get(chatId);
            
            if (chat == null)
            {
                throw new ResourceNotFoundApiException(chatId.ToString(), "Chat");
            }

            return chat;
        }
    }
}