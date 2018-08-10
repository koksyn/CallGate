using CallGate.ApiModels.Authentication;
using CallGate.DependencyInjection;

namespace CallGate.Services.Authentication
{
    public interface IAuthenticationService : ITransientDependency
    {
        TokenResponse GenerateAuthenticationToken(LoginRequest loginRequest);
        
        void Register(RegisterRequest registerRequest);
        
        void ConfirmRegistration(ConfirmRegistrationRequest confirmRegistrationRequest);

        Models.User GetUserByUserNameAndPassword(string login, string password);
    }
}