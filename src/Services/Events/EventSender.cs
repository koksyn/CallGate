using System.Net.WebSockets;
using System.Threading.Tasks;
using CallGate.Documents;
using CallGate.Services.Helper;
using CallGate.Services.Socket;
using CallGate.Stores;

namespace CallGate.Services.Events
{
    public class EventSender : IEventSender
    {
        private readonly IAuthorizedUserHelper _authorizedUserHelper;
        private readonly ISocketStream _socketStream;
        private readonly IEventStore _eventStore;

        public EventSender(
            IAuthorizedUserHelper authorizedUserHelper,
            ISocketStream socketStream,
            IEventStore eventStore
        ) {
            _authorizedUserHelper = authorizedUserHelper;
            _socketStream = socketStream;
            _eventStore = eventStore;
        }

        public async Task SendUserRelatedEvents(WebSocket webSocket)
        {
            var authorizedUser = await _authorizedUserHelper.GetAuthorizedUserAsync();
            
            var cursor = _eventStore.GetChangeFeedCursorForUserId(authorizedUser.Id);
            
            foreach(EventChangeResult result in cursor)
            {
                var @event = result.NewValue;
                
                await _socketStream.SendObjectAsync(webSocket, @event);
            }
        }
    }
}