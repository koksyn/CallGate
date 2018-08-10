using System;
using System.Collections.Generic;
using CallGate.DependencyInjection;
using CallGate.Models;

namespace CallGate.Repositories
{
    public interface IChatRepository : IRepository<Chat>, ITransientDependency
    {
        IEnumerable<Chat> GetAllByUserIdAndGroupId(Guid userId, Guid groupId);

        Chat GetChatWithOnlyTwoUsersByGroupId(Guid userId, Guid secondUserId, Guid groupId);
        
        int CountByGroupId(Guid groupId);
    }
}