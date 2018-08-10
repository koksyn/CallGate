using System;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using CallGate.Services.Events;
using CallGate.Services.Message;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace CallGate.Services.Socket
{
    public class SocketMiddleware : ISocketMiddleware
    {
        private readonly IMessageReceiver _messageReceiver;
        private readonly IMessageSender _messageSender;
        private readonly IEventSender _eventSender;

        public SocketMiddleware(
            IMessageReceiver messageReceiver,
            IMessageSender messageSender, 
            IEventSender eventSender
        ){
            _messageReceiver = messageReceiver;
            _messageSender = messageSender;
            _eventSender = eventSender;
        }

        public async Task Invoke(HttpContext context, Func<Task> next)
        {
            if (NotWebSocketRequest(context))
            {
                await next();
                return;
            }
            
            if (await NotAuthenticated(context))
            {
                context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                return;
            }
            
            if (InvalidEndpoint(context))
            {
                context.Response.StatusCode = (int) HttpStatusCode.NotFound;
                return;
            }
            
            await HandleRequests(context);
        }
        
        private static bool NotWebSocketRequest(HttpContext context)
        {
            return !context.WebSockets.IsWebSocketRequest;
        }
        
        private static async Task<bool> NotAuthenticated(HttpContext context)
        {
            /*
             * Authentication should be called manually
             * https://stackoverflow.com/questions/43135765/websockets-and-authentication-with-identityserver4
             */
            var authenticateResult = await context.AuthenticateAsync("Jwt");
            
            return !authenticateResult.Succeeded;
        }
        
        private static bool InvalidEndpoint(HttpContext context)
        {
            var endpoints = new[] { "/messages/send", "/messages/receive", "/events" };

            return !endpoints.Contains(context.Request.Path.Value);
        }

        private async Task HandleRequests(HttpContext context)
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            
            if (context.Request.Path.Value == "/messages/send")
            {
                WebSocketReceiveResult result = await _messageReceiver.ReceiveMessages(webSocket);
                
                var status = result.CloseStatus ?? WebSocketCloseStatus.Empty;
            
                await webSocket.CloseAsync(
                    status, 
                    result.CloseStatusDescription, 
                    default(CancellationToken)
                );
            }
            else if (context.Request.Path.Value == "/messages/receive")
            {
                await _messageSender.SendUserRelatedMessages(webSocket);
                
                await webSocket.CloseAsync(
                    WebSocketCloseStatus.Empty, 
                    "closed successfully",
                    default(CancellationToken)
                );
            }
            else // "/events"
            {
                await _eventSender.SendUserRelatedEvents(webSocket);
                
                await webSocket.CloseAsync(
                    WebSocketCloseStatus.Empty, 
                    "success",
                    default(CancellationToken)
                );
            }
        }
    }
}