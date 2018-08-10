using System.Collections.Generic;
using CallGate.Documents;
using CallGate.Services.Helper;
using CallGate.Stores;

namespace CallGate.Services.Events
{
    public class EventService : IEventService
    {
        private readonly IAuthorizedUserHelper _authorizedUserHelper;
        private readonly IEventStore _eventStore;

        public EventService(IAuthorizedUserHelper authorizedUserHelper, IEventStore eventStore)
        {
            _authorizedUserHelper = authorizedUserHelper;
            _eventStore = eventStore;
        }

        public IEnumerable<Event> GetAllForAuthorizedUser()
        {
            var authorizedUserId = _authorizedUserHelper.GetAuthorizedUserId();

            return _eventStore.GetAllForUserId(authorizedUserId);
        }
    }
}