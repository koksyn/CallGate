using AutoMapper;
using CallGate.ApiModels.User;
using CallGate.Repositories;
using CallGate.Services.Email;
using CallGate.Services.Helper;

namespace CallGate.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        private readonly Models.User _authorizedUser;

        public UserService(
            IAuthorizedUserHelper authorizedUserHelper,
            IUserRepository userRepository,
            IMailService mailService,
            IMapper mapper
        )
        {
            _userRepository = userRepository;
            _mailService = mailService;
            _mapper = mapper;
            _authorizedUser = authorizedUserHelper.GetAuthorizedUser();
        }

        public UserFullResponse GetAuthorizedUser()
        {
            return _mapper.Map<UserFullResponse>(_authorizedUser);
        }

        public void RemoveAuthorizedUser()
        {
            _userRepository.Remove(_authorizedUser);
            
            _mailService.SendMail(
                _authorizedUser.Email, 
                "CallGate - Your account was successfully deleted", 
                $"Hi {_authorizedUser.Username}, we have removed your account on your demand. " +
                "We hope you will come back to us again!"
            );
        }
    }
}