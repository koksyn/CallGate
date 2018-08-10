using System;
using System.Collections.Generic;
using CallGate.DependencyInjection;
using CallGate.Models;

namespace CallGate.Repositories
{
    public interface IGroupUserRepository : IRepository<GroupUser>, ITransientDependency
    {
        GroupUser GetByUserIdAndGroupId(Guid userId, Guid groupId);
        
        GroupUser GetByUserIdAndGroupIdAndRole(Guid userId, Guid groupId, Role role);
        
        IEnumerable<GroupUser> GetAllByUsernameAndGroupId(string username, Guid groupId);
        
        IEnumerable<GroupUser> GetAllByUsernameAndGroupIdOutsideChatId(string username, Guid groupId, Guid chatId);
        
        IEnumerable<GroupUser> GetAllByUsernameAndGroupIdOutsideChannelId(string username, Guid groupId, Guid channelId);
    }
}