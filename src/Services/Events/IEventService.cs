using System.Collections.Generic;
using CallGate.DependencyInjection;
using CallGate.Documents;

namespace CallGate.Services.Events
{
    public interface IEventService : ITransientDependency
    {
        IEnumerable<Event> GetAllForAuthorizedUser();
    }
}