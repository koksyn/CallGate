using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CallGate.Exceptions;
using CallGate.Repositories;
using Microsoft.AspNetCore.Http;

namespace CallGate.Services.Helper
{
    public class AuthorizedUserHelper : IAuthorizedUserHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public AuthorizedUserHelper(
            IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository
        ){
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public Guid GetAuthorizedUserId()
        {
            var claimUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if(claimUserId == null)
            {
                throw new UserNotLoggedApiException();
            }

            return new Guid(claimUserId.Value);
        }

        public Models.User GetAuthorizedUser()
        {
            var userId = GetAuthorizedUserId();
            
            return _userRepository.Get(userId);
        }
        
        public Task<Models.User> GetAuthorizedUserAsync()
        {
            var userId = GetAuthorizedUserId();
            
            return _userRepository.GetUserAsync(userId);
        }
    }
}