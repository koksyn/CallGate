using CallGate.DependencyInjection;

namespace CallGate.Services.Channel
{
    public interface IChannelEventService : ITransientDependency
    {        
        void AddChannelCreatedEvent(Models.Group group, Models.User authorUser, Models.Channel channel);
        
        void AddChannelRemovedEvent(Models.Group group, Models.User authorUser, Models.Channel channel);
        
        void AddUserAddedToChannelEvent(Models.Group group, Models.User authorUser, Models.User user, Models.Channel channel);
        
        void AddUserRemovedFromChannelEvent(Models.Group group, Models.User authorUser, Models.User user, Models.Channel channel);
    }
}