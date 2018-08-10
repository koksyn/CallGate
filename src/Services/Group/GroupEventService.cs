using CallGate.Documents;
using CallGate.Models;
using CallGate.Stores;

namespace CallGate.Services.Group
{
    public class GroupEventService : IGroupEventService
    {
        private readonly IEventStore _eventStore;

        public GroupEventService(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public void AddGroupCreatedEvent(Models.Group group, Models.User authorUser)
        {
            var @event = new Event(EventType.GroupCreated, authorUser, group);

            _eventStore.AddToBus(@event);
        }

        public void AddGroupEditedEvent(Models.Group group, Models.User authorUser)
        {
            var @event = new Event(EventType.GroupEdited, authorUser, group);

            _eventStore.AddToBus(@event);
        }

        public void AddUserAddedToGroupEvent(Models.Group group, Models.User authorUser, Models.User user)
        {
            var @event = new Event(EventType.UserAddedToGroup, authorUser, group);
            
            @event.AttachUser(user);

            _eventStore.AddToBus(@event);
        }

        public void AddUserRemovedFromGroupEvent(Models.Group @group, Models.User authorUser, Models.User user)
        {
            var @event = new Event(EventType.UserRemovedFromGroup, authorUser, group);
            
            @event.AttachUser(user);

            _eventStore.AddToBus(@event);
        }

        public void AddRoleInGroupGrantedEvent(Models.Group group, Models.User authorUser, Models.User user, Role role)
        {
            var eventType = (role == Role.Admin)
                ? EventType.AdminRoleInGroupGranted
                : EventType.MemberRoleInGroupGranted;
            
            var @event = new Event(eventType, authorUser, group);
            
            @event.AttachUser(user);

            _eventStore.AddToBus(@event);
        }
    }
}