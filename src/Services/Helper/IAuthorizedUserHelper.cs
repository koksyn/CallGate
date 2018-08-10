using System;
using System.Threading.Tasks;
using CallGate.DependencyInjection;

namespace CallGate.Services.Helper
{
    public interface IAuthorizedUserHelper : IScopedDependency
    {
        Guid GetAuthorizedUserId();
        
        Models.User GetAuthorizedUser();

        Task<Models.User> GetAuthorizedUserAsync();
    }
}