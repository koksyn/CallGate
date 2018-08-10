using System;
using CallGate.DependencyInjection;
using CallGate.Documents;

namespace CallGate.Stores
{
    public interface IGroupUserStore : IStore<GroupUser>, IScopedDependency
    {
        GroupUser GetByUserIdAndGroupId(Guid userId, Guid groupId);

        void RemoveByUserIdAndGroupId(Guid userId, Guid groupId);
    }
}