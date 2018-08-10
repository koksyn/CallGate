using CallGate.Documents;
using CallGate.Stores;

namespace CallGate.Services.Chat
{
    public class ChatEventService : IChatEventService
    {
        private readonly IEventStore _eventStore;

        public ChatEventService(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public void AddChatCreatedEvent(Models.Group group, Models.User authorUser, Models.Chat chat)
        {
            var @event = new Event(EventType.ChatCreated, authorUser, group);
            
            @event.AttachChat(chat);

            _eventStore.AddToBus(@event);
        }

        public void AddChatRemovedEvent(Models.Group @group, Models.User authorUser, Models.Chat chat)
        {
            var @event = new Event(EventType.ChatRemoved, authorUser, group);
            
            @event.AttachChat(chat);

            _eventStore.AddToBus(@event);
        }

        public void AddUserAddedToChatEvent(Models.Group group, Models.User authorUser, Models.User user, Models.Chat chat)
        {
            var @event = new Event(EventType.UserAddedToChat, authorUser, group);
            
            @event.AttachUser(user);
            @event.AttachChat(chat);

            _eventStore.AddToBus(@event);
        }

        public void AddUserRemovedFromChatEvent(Models.Group group, Models.User authorUser, Models.User user, Models.Chat chat)
        {
            var @event = new Event(EventType.UserRemovedFromChat, authorUser, group);
            
            @event.AttachUser(user);
            @event.AttachChat(chat);

            _eventStore.AddToBus(@event);
        }
    }
}