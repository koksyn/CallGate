using CallGate.DependencyInjection;
using CallGate.Models;

namespace CallGate.Services.Group
{
    public interface IGroupEventService : ITransientDependency
    {
        void AddGroupCreatedEvent(Models.Group group, Models.User authorUser);
        
        void AddGroupEditedEvent(Models.Group group, Models.User authorUser);
        
        void AddUserAddedToGroupEvent(Models.Group group, Models.User authorUser, Models.User user);
        
        void AddUserRemovedFromGroupEvent(Models.Group group, Models.User authorUser, Models.User user);
        
        void AddRoleInGroupGrantedEvent(Models.Group group, Models.User authorUser, Models.User user, Role role);
    }
}