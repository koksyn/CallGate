using CallGate.Documents;
using CallGate.Stores;

namespace CallGate.Services.Channel
{
    public class ChannelEventService : IChannelEventService
    {
        private readonly IEventStore _eventStore;

        public ChannelEventService(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public void AddChannelCreatedEvent(Models.Group @group, Models.User authorUser, Models.Channel channel)
        {
            var @event = new Event(EventType.ChannelCreated, authorUser, group);
            
            @event.AttachChannel(channel);

            _eventStore.AddToBus(@event);
        }

        public void AddChannelRemovedEvent(Models.Group @group, Models.User authorUser, Models.Channel channel)
        {
            var @event = new Event(EventType.ChannelRemoved, authorUser, group);
            
            @event.AttachChannel(channel);

            _eventStore.AddToBus(@event);
        }

        public void AddUserAddedToChannelEvent(Models.Group @group, Models.User authorUser, Models.User user, Models.Channel channel)
        {
            var @event = new Event(EventType.UserAddedToChannel, authorUser, group);
            
            @event.AttachUser(user);
            @event.AttachChannel(channel);

            _eventStore.AddToBus(@event);
        }

        public void AddUserRemovedFromChannelEvent(Models.Group group, Models.User authorUser, Models.User user, Models.Channel channel)
        {
            var @event = new Event(EventType.UserRemovedFromChannel, authorUser, group);
            
            @event.AttachUser(user);
            @event.AttachChannel(channel);

            _eventStore.AddToBus(@event);
        }
    }
}