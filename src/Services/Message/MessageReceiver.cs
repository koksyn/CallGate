using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using CallGate.ApiModels.Socket;
using CallGate.Services.Channel;
using CallGate.Services.Chat;
using CallGate.Services.Helper;
using CallGate.Services.Socket;

namespace CallGate.Services.Message
{
    public class MessageReceiver : IMessageReceiver
    {
        private readonly IAuthorizedUserHelper _authorizedUserHelper;
        private readonly IChannelMessageService _channelMessageService;
        private readonly IChatMessageService _chatMessageService;
        private readonly ISocketStream _socketStream;

        public MessageReceiver(
            IAuthorizedUserHelper authorizedUserHelper,
            IChannelMessageService channelMessageService,
            IChatMessageService chatMessageService,
            ISocketStream socketStream
        ){
            _authorizedUserHelper = authorizedUserHelper;
            _channelMessageService = channelMessageService;
            _chatMessageService = chatMessageService;
            _socketStream = socketStream;
        }

        public async Task<WebSocketReceiveResult> ReceiveMessages(WebSocket webSocket)
        {
            // get authorized user only once before starting loop
            var authorizedUser = await _authorizedUserHelper.GetAuthorizedUserAsync();
            
            var socketMessage = await _socketStream.ReceiveAsync(webSocket);
            
            while (!socketMessage.Result.CloseStatus.HasValue)
            {
                var processingResult = await ProcessSocketMessage(socketMessage, authorizedUser);
                
                // send processing result back to the User
                await _socketStream.SendObjectAsync(webSocket, processingResult);
                
                // receive/wait for next message
                socketMessage = await _socketStream.ReceiveAsync(webSocket);
            }

            return socketMessage.Result;
        }

        private async Task<object> ProcessSocketMessage(SocketMessage<MessageRequest> socketMessage, Models.User authorizedUser)
        {
            var command = socketMessage.GetObject();

            RequireNotEmptyContent(command);
            RequireOnlyOneId(command);

            Documents.Message message;
            
            if (command.ChatId != Guid.Empty)
            {
                message = await _chatMessageService.AddMessageToChatAsync(command.ChatId, command.Content, authorizedUser);
            }
            else
            {
                message = await _channelMessageService.AddMessageToChannelAsync(command.ChannelId, command.Content, authorizedUser);
            }

            return message;
        }

        private static void RequireNotEmptyContent(MessageRequest command)
        {
            if (string.IsNullOrEmpty(command.Content))
            {
                throw new Exception("Expected non-empty message!");
            }
        }
        
        private static void RequireOnlyOneId(MessageRequest command)
        {
            if (command.ChatId != Guid.Empty && command.ChannelId != Guid.Empty)
            {
                throw new Exception("You cannot send the message to both: Channel and Chat");
            }
            
            if (command.ChatId == Guid.Empty && command.ChannelId == Guid.Empty)
            {
                throw new Exception("You should provide an ID for Chat or Channel");
            }
        }
    }
}