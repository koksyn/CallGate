using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CallGate.ApiModels.ChatMessage;
using CallGate.Stores;

namespace CallGate.Services.Chat
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly IChatUserStore _chatUserStore;
        private readonly IMessageStore _messageStore;
        private readonly IMapper _mapper;

        public ChatMessageService(
            IChatUserStore chatUserStore,
            IMessageStore messageStore, 
            IMapper mapper)
        {
            _chatUserStore = chatUserStore;
            _messageStore = messageStore;
            _mapper = mapper;
        }

        public IEnumerable<ChatMessageResponse> GetAllByChatId(Guid chatId)
        {
            var chatMessages = _messageStore.GetAllChatMessagesByChatId(chatId);
            
            return _mapper.Map<IEnumerable<ChatMessageResponse>>(chatMessages);
        }

        public Task<Documents.Message> AddMessageToChatAsync(Guid chatId, string content, Models.User user)
        {
            RequireUserIsChatMember(user.Id, chatId);
            
            var message = new Documents.Message
            {
                Id = Guid.NewGuid(),
                Content = content,
                Created = DateTime.Now,
                ChatId = chatId,
                UserId = user.Id,
                Username = user.Username
            };
            
            _messageStore.Add(message);

            return Task.FromResult(message);
        }

        private void RequireUserIsChatMember(Guid userId, Guid chatId)
        {
            var chatUser = _chatUserStore.GetOneByUserIdAndChatId(userId, chatId);

            if (chatUser == null)
            {
                throw new Exception("You are not a member of this chat.");
            }
        }
    }
}