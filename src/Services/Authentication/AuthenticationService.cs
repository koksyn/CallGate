using System;
using CallGate.ApiModels.Authentication;
using CallGate.Repositories;
using CallGate.Services.Email;
using CallGate.Services.Helper;

namespace CallGate.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashService _hashService;
        private readonly IJwtHelper _jwtHelper;
        private readonly IMailService _mailService;
        
        public AuthenticationService(
            IUserRepository userRepository,
            IHashService hashService,
            IJwtHelper jwtHelper,
            IMailService mailService)
        {
            _userRepository = userRepository;
            _hashService = hashService;
            _jwtHelper = jwtHelper;
            _mailService = mailService;
        }

        public TokenResponse GenerateAuthenticationToken(LoginRequest command)
        {
            var user = GetUserByUserNameAndPassword(command.Username, command.Password);
            
            var notBeforeDate = DateTime.Now;
            var expriationDate = notBeforeDate.AddHours(5);

            var token = _jwtHelper.GenerateToken(user.Id, user.Username, notBeforeDate, expriationDate);

            return new TokenResponse
            {
                Token = token,
                ExpirationDate = expriationDate
            };
        }

        public void Register(RegisterRequest command)
        {
            var confirmationPhrase = Guid.NewGuid().ToString();

            var user = new Models.User
            {
                Id = Guid.NewGuid(),
                Username = command.Username,
                Password = _hashService.GenerateHash(command.Password),
                Email = command.Email,
                IsActive = false,
                ConfirmationPhrase = confirmationPhrase
            };

            _userRepository.Add(user);
            
            var confimationUrl = $"{command.RedirectUrl}?confirmationPhrase={confirmationPhrase}";

            _mailService.SendMail(
                command.Email, 
                "CallGate registration", 
                $"In order to finish registration click {confimationUrl}."
            );
        }

        public void ConfirmRegistration(ConfirmRegistrationRequest command)
        {
            var user = _userRepository.GetUserByConfirmationPhrase(command.ConfirmationPhrase);
            
            user.IsActive = true;
        }

        public Models.User GetUserByUserNameAndPassword(string login, string password)
        {
            var hashedPassword = _hashService.GenerateHash(password);
            
            return _userRepository.GetByUsernameAndPassword(login, hashedPassword);
        }
    }
}
