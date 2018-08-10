using CallGate.Exceptions;
using CallGate.Repositories;
using CallGate.Services.Authentication;
using CallGate.Services.Helper;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CallGate.Services.User
{
    public class UserValidationService : IUserValidationService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserRepository _userRepository;
        private readonly IAuthorizedUserHelper _authorizedUserHelper;

        public UserValidationService(
            IAuthenticationService authenticationService,
            IAuthorizedUserHelper authorizedUserHelper,
            IUserRepository userRepository
        )
        {
            _authenticationService = authenticationService;
            _userRepository = userRepository;
            _authorizedUserHelper = authorizedUserHelper;
        }
        
        public Models.User RequireAndGetUserByUsername(string username)
        {
            var user = _userRepository.GetUserByUsername(username);
                
            if (user == null)
            {
                throw new ResourceNotFoundApiException(username, "User", "username");
            }

            return user;
        }

        public void RequireValidPasswordForAuthorizedUser(string password)
        {
            var authorizedUser = _authorizedUserHelper.GetAuthorizedUser();
            var user = _authenticationService.GetUserByUserNameAndPassword(authorizedUser.Username, password);
                
            if (user == null)
            {
                throw new ParseApiException("Invalid password");
            }
        }
        
        public void RequireValidPasswordForExistingUsername(string password, string username)
        {
            var user = _authenticationService.GetUserByUserNameAndPassword(username, password);
                
            if (user == null || !user.IsActive)
            {
                throw new ParseApiException("Invalid username or password");
            }
        }

        public void RequireUniqueUsernameAndEmail(string username, string email, ModelStateDictionary modelState)
        {
            var userByUsername = _userRepository.GetUserByUsername(username);
            var userByEmail = _userRepository.GetUserByEmail(email);
                
            if (userByUsername != null)
            {
                modelState.AddModelError("Username", $"There is already user with username {username}");
            }

            if (userByEmail != null)
            {
                modelState.AddModelError("Email", $"There is already user with email {email}");
            }
        }

        public void RequireValidConfirmationPhraseForInactiveUser(string confirmationPhrase)
        {
            var user = _userRepository.GetUserByConfirmationPhrase(confirmationPhrase);
            
            if (user == null || user.IsActive)
            {
                throw new ParseApiException("User not found or is already active");
            }
        }
    }
}