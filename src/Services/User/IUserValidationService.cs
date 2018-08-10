using CallGate.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CallGate.Services.User
{
    public interface IUserValidationService : ITransientDependency
    {
        Models.User RequireAndGetUserByUsername(string username);
        
        void RequireValidPasswordForAuthorizedUser(string password);

        void RequireValidPasswordForExistingUsername(string password, string username);
        
        void RequireUniqueUsernameAndEmail(string username, string email, ModelStateDictionary modelState);
        
        void RequireValidConfirmationPhraseForInactiveUser(string confirmationPhrase);
    }
}