using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CallGate.Data;
using CallGate.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CallGate.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public UserRepository(DatabaseContext databaseContext, IServiceScopeFactory scopeFactory) : base(databaseContext)
        {
            _scopeFactory = scopeFactory;
        }

        public User GetByUsernameAndPassword(string username, string hashedPassword)
        {
            return DbSet
                .Where(user => user.Username == username)
                .SingleOrDefault(user => user.Password == hashedPassword);
        }
        
        public async Task<User> GetUserAsync(Guid userId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var databaseContext = scope.ServiceProvider.GetService<DatabaseContext>();
                var dbSet = databaseContext.Set<User>();
                
                return await dbSet.FindAsync(userId);
            }
        }

        public User GetUserByUsername(string username)
        {
            return DbSet
                .SingleOrDefault(user => user.Username == username);
        }

        public User GetUserByEmail(string email)
        {
            return DbSet
                .SingleOrDefault(user => user.Email == email);
        }

        public User GetUserByConfirmationPhrase(string confirmationPhrase)
        {
            return DbSet
                .SingleOrDefault(user => user.ConfirmationPhrase == confirmationPhrase);
        }

        public IEnumerable<User> GetAllUsersByGroupIdAndUsername(Guid groupId, string username)
        {
            return DbSet
                .Where(u => ContainsWhenNotEmpty(u.Username, username))
                .SelectMany(gu => gu.GroupUsers)
                .Where(gu => gu.GroupId == groupId)
                .Select(gu => gu.User)
                .Distinct()
                .ToList();
        }

        public IEnumerable<User> GetAllUsersByChatIdAndUsername(Guid chatId, string username)
        {
            return DbSet
                .Where(u => ContainsWhenNotEmpty(u.Username, username))
                .SelectMany(cu => cu.ChatUsers)
                .Where(cu => cu.ChatId == chatId)
                .Select(cu => cu.User)
                .Distinct()
                .ToList();
        }

        public IEnumerable<User> GetAllUsersByChannelIdAndUsername(Guid channelId, string username)
        {
            return DbSet
                .Where(u => ContainsWhenNotEmpty(u.Username, username))
                .SelectMany(cu => cu.ChannelUsers)
                .Where(cu => cu.ChannelId == channelId)
                .Select(cu => cu.User)
                .Distinct()
                .ToList();
        }
        
        public IEnumerable<User> GetAllFromChatsConnectedWithUserIdByGroupIdAndUsersCount(Guid userId, Guid groupId, int usersCount)
        {
            var chatsQuery = DbContext.Chats
                .Where(c => c.GroupId == groupId)
                .Include(c => c.ChatUsers)
                .ThenInclude(cu => cu.User)
                .Where(c => c.ChatUsers.Count == usersCount)
                .Where(c => c.ChatUsers.Any(cu => cu.UserId == userId));

            var chatUsersWithoutUserId = chatsQuery
                .SelectMany(cu => cu.ChatUsers)
                .Where(cu => cu.UserId != userId)
                .Include(cu => cu.User)
                .ToList();

            var users = new List<User>();
            
            foreach (var chatUser in chatUsersWithoutUserId)
            {
                users.Add(chatUser.User);
            }

            return users
                .Distinct()
                .AsEnumerable();
        }

        public IEnumerable<User> GetAllByUsernameExceptEnumerable(string username, IEnumerable<User> exceptUsers)
        {
            return DbSet
                .Where(u => u.Username.Contains(username))
                .Except(exceptUsers)
                .ToList();
        }

        public IEnumerable<User> GetAllByGroupIdExceptEnumerableAndUserId(Guid groupId, IEnumerable<User> exceptUsers, Guid userId)
        {
            return DbSet
                .SelectMany(gu => gu.GroupUsers)
                .Where(gu => gu.GroupId == groupId)
                .Select(gu => gu.User)
                .Where(u => u.Id != userId)
                .Distinct()
                .Except(exceptUsers)
                .ToList();
        }
    }
}
