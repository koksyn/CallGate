using System;
using System.Collections.Generic;
using System.Linq;
using CallGate.Data;
using CallGate.Models;
using Microsoft.EntityFrameworkCore;

namespace CallGate.Repositories
{
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        public GroupRepository(DatabaseContext databaseContext) : base(databaseContext) {}

        public Group GetOneByName(string name)
        {
            return DbSet
                .SingleOrDefault(group => group.Name == name);
        }

        public Group GetByUserId(Guid groupId, Guid userId)
        {
            var userGroup = DbContext.Users
                .Where(u => u.Id == userId)
                .SelectMany(gu => gu.GroupUsers)
                .Include(gu => gu.Group)
                .SingleOrDefault(gu => gu.GroupId == groupId);

            return userGroup?.Group;
        }
        
        public Group GetByRoleAndUserId(Guid groupId, Role role, Guid userId)
        {
            var userGroup = DbContext.Users
                .Where(u => u.Id == userId)
                .SelectMany(gu => gu.GroupUsers)
                .Include(gu => gu.Group)
                .Where(gu => gu.Role == role)
                .SingleOrDefault(gu => gu.GroupId == groupId);

            return userGroup?.Group;
        }

        public IEnumerable<Group> GetAllByUserId(Guid userId)
        {
            return DbContext.Users
                .Where(u => u.Id == userId)
                .SelectMany(gu => gu.GroupUsers)
                .Select(gu => gu.Group)
                .OrderBy(group => group.Name)
                .ToList();
        }
    }
}