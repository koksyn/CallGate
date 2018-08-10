using System.Net.WebSockets;
using System.Threading.Tasks;
using CallGate.DependencyInjection;

namespace CallGate.Services.Events
{
    public interface IEventSender : IScopedDependency
    {
        Task SendUserRelatedEvents(WebSocket webSocket);
    }
}