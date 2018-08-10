using System;
using System.Collections.Generic;
using System.Linq;
using CallGate.Data;
using CallGate.Models;
using Microsoft.EntityFrameworkCore;

namespace CallGate.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly DbSet<TEntity> DbSet;
        protected readonly DatabaseContext DbContext;

        protected Repository(DatabaseContext databaseContext)
        {
            DbContext = databaseContext;
            DbSet = DbContext.Set<TEntity>();
        }

        public bool Any()
        {
            return DbSet.Any();
        }

        public TEntity Add(TEntity entity)
        {
            var createdEntity = DbSet.Add(entity);

            return createdEntity.Entity;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return DbSet.ToList();
        }

        public virtual TEntity Get(Guid id)
        {
            return DbSet.Find(id);
        }

        public void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
        }
        
        protected bool ContainsWhenNotEmpty(string field, string phrase)
        {
            if (string.IsNullOrEmpty(phrase))
            {
                return true;
            }

            field = field.ToLower();
            phrase = phrase.ToLower();

            return field.Contains(phrase);
        }
    }
}
