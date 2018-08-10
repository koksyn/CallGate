using System;
using System.Collections.Generic;
using CallGate.DependencyInjection;
using CallGate.Models;

namespace CallGate.Repositories
{
    public interface IGroupRepository : IRepository<Group>, ITransientDependency
    {
        Group GetOneByName(string name);

        Group GetByUserId(Guid id, Guid userId);

        Group GetByRoleAndUserId(Guid groupId, Role role, Guid userId);

        IEnumerable<Group> GetAllByUserId(Guid userId);
    }
}