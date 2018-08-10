using System.Net.WebSockets;
using System.Threading.Tasks;
using CallGate.DependencyInjection;

namespace CallGate.Services.Message
{
    public interface IMessageReceiver : IScopedDependency
    {
        Task<WebSocketReceiveResult> ReceiveMessages(WebSocket webSocket);
    }
}