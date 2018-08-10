using System;
using System.Collections.Generic;
using CallGate.Models;

namespace CallGate.Repositories
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        bool Any();
        TEntity Add(TEntity entity);
        IEnumerable<TEntity> GetAll();
        TEntity Get(Guid id);
        void Remove(TEntity entity);
    }
}