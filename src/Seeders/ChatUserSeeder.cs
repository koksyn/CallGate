using System.Linq;
using CallGate.Documents;
using CallGate.Repositories;
using CallGate.Services.Chat;
using CallGate.Stores;

namespace CallGate.Seeders
{
    public class ChatUserSeeder : BaseSeeder
    {
        private readonly IChatEventService _chatEventService;
        private readonly IChatUserStore _chatUserStore;
        private readonly IChatUserRepository _chatUserRepository;
        private readonly IGroupRepository _groupRepository;

        public ChatUserSeeder(
            IChatEventService chatEventService, 
            IChatUserStore chatUserStore, 
            IChatUserRepository chatUserRepository,
            IGroupRepository groupRepository)
        {
            _chatEventService = chatEventService;
            _chatUserStore = chatUserStore;
            _chatUserRepository = chatUserRepository;
            _groupRepository = groupRepository;
        }

        public override void Seed()
        {
            if (_chatUserStore.Any() || !_chatUserRepository.Any())
            {
                return;
            }

            var chatUsers = _chatUserRepository.GetAll();
            var author = chatUsers.First().User;

            foreach(var chatUser in chatUsers) 
            {
                _chatUserStore.Add(new ChatUser(chatUser.UserId, chatUser.ChatId));
                
                var group = _groupRepository.Get(chatUser.Chat.GroupId);
                _chatEventService.AddUserAddedToChatEvent(group, author, chatUser.User, chatUser.Chat);
            }
        }
        
        public override int GetPriority()
        {
            return 6;
        }
    }
}