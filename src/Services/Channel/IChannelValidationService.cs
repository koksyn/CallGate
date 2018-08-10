using System;
using CallGate.DependencyInjection;

namespace CallGate.Services.Channel
{
    public interface IChannelValidationService : ITransientDependency
    {
        void RequireUserIsChannelMember(string username, Guid channelId);
        
        void RequireUserIsNotChannelMember(string username, Guid channelId);
        
        void RequireAuthorizedUserIsNotChannelMember(Guid channelId);
        
        void RequireAuthorizedUserIsChannelMember(Guid channelId);

        void RequireUserIsGroupMemberFromChannel(string username, Guid channelId);
        
        void RequireAuthorizedUserIsGroupMemberFromChannel(Guid channelId);
        
        void RequireAuthorizedUserIsGroupAdminFromChannel(Guid channelId);
    }
}