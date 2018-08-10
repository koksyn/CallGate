using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using AutoMapper;
using CallGate.Documents;
using CallGate.Services.Helper;
using CallGate.Services.Socket;
using CallGate.Stores;

namespace CallGate.Services.Message
{
    public class MessageSender : IMessageSender
    {
        private readonly IAuthorizedUserHelper _authorizedUserHelper;
        private readonly ISocketStream _socketStream;
        private readonly IMessageStore _messageStore;
        private readonly IMapper _mapper;

        public MessageSender(IAuthorizedUserHelper authorizedUserHelper, ISocketStream socketStream, IMessageStore messageStore, IMapper mapper)
        {
            _authorizedUserHelper = authorizedUserHelper;
            _socketStream = socketStream;
            _messageStore = messageStore;
            _mapper = mapper;
        }

        public async Task SendUserRelatedMessages(WebSocket webSocket)
        {
            // get authorized user only once before starting loop
            var authorizedUser = await _authorizedUserHelper.GetAuthorizedUserAsync();
            
            var cursor = _messageStore.GetChangeFeedCursorForUserId(authorizedUser.Id);
            
            foreach(MessageChangeResult result in cursor)
            {
                var chatId = result.NewValue.ChatId;
                var channelId = result.NewValue.ChannelId;
                
                if (chatId != Guid.Empty)
                {
                    var chatMessage = _mapper.Map<ChatMessage>(result.NewValue);
                    await _socketStream.SendObjectAsync(webSocket, chatMessage);
                }
                else if (channelId != Guid.Empty)
                {
                    var channelMessage = _mapper.Map<ChannelMessage>(result.NewValue);
                    await _socketStream.SendObjectAsync(webSocket, channelMessage);
                }
            }
        }
    }
}