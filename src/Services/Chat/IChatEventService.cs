using CallGate.DependencyInjection;

namespace CallGate.Services.Chat
{
    public interface IChatEventService : ITransientDependency
    {
        void AddChatCreatedEvent(Models.Group group, Models.User authorUser, Models.Chat chat);
        
        void AddChatRemovedEvent(Models.Group group, Models.User authorUser, Models.Chat chat);
        
        void AddUserAddedToChatEvent(Models.Group group, Models.User authorUser, Models.User user, Models.Chat chat);
        
        void AddUserRemovedFromChatEvent(Models.Group group, Models.User authorUser, Models.User user, Models.Chat chat);
    }
}