using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CallGate.DependencyInjection;
using CallGate.Models;

namespace CallGate.Repositories
{
    public interface IUserRepository : IRepository<User>, IScopedDependency
    {
        User GetByUsernameAndPassword(string username, string password);
        Task<User> GetUserAsync(Guid userId);
        User GetUserByUsername(string username);
        User GetUserByEmail(string email);
        User GetUserByConfirmationPhrase(string confirmationPhrase);
        IEnumerable<User> GetAllUsersByGroupIdAndUsername(Guid groupId, string username);
        IEnumerable<User> GetAllUsersByChatIdAndUsername(Guid chatId, string username);
        IEnumerable<User> GetAllUsersByChannelIdAndUsername(Guid channelId, string username);
        IEnumerable<User> GetAllFromChatsConnectedWithUserIdByGroupIdAndUsersCount(
            Guid userId, 
            Guid groupId,
            int usersCount
        );
        IEnumerable<User> GetAllByUsernameExceptEnumerable(string username, IEnumerable<User> exceptUsers);
        IEnumerable<User> GetAllByGroupIdExceptEnumerableAndUserId(Guid groupId, IEnumerable<User> exceptUsers, Guid userId);
    }
}