using CallGate.ApiModels.User;
using CallGate.DependencyInjection;

namespace CallGate.Services.User
{
    public interface IUserService : ITransientDependency
    {
        UserFullResponse GetAuthorizedUser();
        void RemoveAuthorizedUser();
    }
}