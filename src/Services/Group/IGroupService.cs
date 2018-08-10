using System;
using System.Collections.Generic;
using CallGate.ApiModels.Group;
using CallGate.DependencyInjection;

namespace CallGate.Services.Group
{
    public interface IGroupService : ITransientDependency
    {
        IEnumerable<GroupResponse> GetAll();

        GroupResponse Get(Guid groupId);

        GroupResponse GetOneByName(string name);
        
        GroupResponse Create(string name, string description);
        
        void Edit(Guid groupId, string name, string description);
    }
}