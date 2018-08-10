using System;
using System.Collections.Generic;
using CallGate.DependencyInjection;
using CallGate.Documents;
using RethinkDb.Driver.Net;

namespace CallGate.Stores
{
    public interface IEventStore : IStore<Event>, IScopedDependency
    {
        IEnumerable<Event> GetAllForUserId(Guid userId);
        
        Cursor<EventChangeResult> GetChangeFeedCursorForUserId(Guid userId);
    }
}