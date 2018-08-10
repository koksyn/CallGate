using System.Net.WebSockets;
using System.Threading.Tasks;
using CallGate.DependencyInjection;

namespace CallGate.Services.Message
{
    public interface IMessageSender : IScopedDependency
    {
        Task SendUserRelatedMessages(WebSocket webSocket);
    }
}