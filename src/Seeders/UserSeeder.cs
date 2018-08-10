using System;
using CallGate.Models;
using CallGate.Repositories;
using CallGate.Services.Authentication;

namespace CallGate.Seeders
{
    public class UserSeeder : BaseSeeder
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashService _hashService;

        public UserSeeder(IUserRepository userRepository, IHashService hashService)
        {
            _userRepository = userRepository;
            _hashService = hashService;
        }

        public override void Seed()
        {
            if (_userRepository.Any())
            {
                return;
            }
            
            _userRepository.Add(new User
            {
                Username = "Admin",
                Password = _hashService.GenerateHash("admin"),
                Email = "admin@callgate.pl",
                ConfirmationPhrase = Guid.NewGuid().ToString(),
                IsActive = true
            });
            
            _userRepository.Add(new User
            {
                Username = "Ciri",
                Password = _hashService.GenerateHash("ciri"),
                Email = "ciri@callgate.pl",
                ConfirmationPhrase = Guid.NewGuid().ToString(),
                IsActive = true
            });
            
            _userRepository.Add(new User
            {
                Username = "Geralt",
                Password = _hashService.GenerateHash("geralt"),
                Email = "geralt@callgate.pl",
                ConfirmationPhrase = Guid.NewGuid().ToString(),
                IsActive = true
            });
        }

        public override int GetPriority()
        {
            return 1;
        }
    }
}
