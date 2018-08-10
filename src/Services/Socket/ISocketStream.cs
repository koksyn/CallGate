using System.Net.WebSockets;
using System.Threading.Tasks;
using CallGate.ApiModels.Socket;
using CallGate.DependencyInjection;

namespace CallGate.Services.Socket
{
    public interface ISocketStream : IScopedDependency
    {
        Task<SocketMessage<MessageRequest>> ReceiveAsync(WebSocket webSocket);
        
        Task SendObjectAsync(WebSocket webSocket, object obj);
        
        Task SendStringAsync(WebSocket webSocket, string rawMessage);
    }
}