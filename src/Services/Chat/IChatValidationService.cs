using System;
using CallGate.DependencyInjection;

namespace CallGate.Services.Chat
{
    public interface IChatValidationService : ITransientDependency
    {
        void RequireDifferentUsersForChatCreation(string username);
        
        void RequireChatBetweenUsersInGroupDoesNotExist(string username, Guid groupId);
        
        void RequireUserIsNotChatMember(string username, Guid chatId);
        
        void RequireAuthorizedUserIsChatMember(Guid chatId);

        void RequireUserIsGroupMemberFromChat(string username, Guid chatId);
        
        void RequireAuthorizedUserIsGroupMemberFromChat(Guid chatId);
    }
}