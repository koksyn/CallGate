using System;
using System.Linq;
using CallGate.Data;
using CallGate.Models;
using CallGate.Repositories;
using CallGate.Services.Chat;
using Microsoft.EntityFrameworkCore;

namespace CallGate.Seeders
{
    public class ChatSeeder : BaseSeeder
    {
        private readonly IChatEventService _chatEventService;
        private readonly IGroupRepository _groupRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IChatUserRepository _chatUserRepository;
        private readonly IUserRepository _userRepository;

        public ChatSeeder(
            IChatEventService chatEventService,
            IGroupRepository groupRepository,
            IChatRepository chatRepository, 
            IChatUserRepository chatUserRepository, 
            IUserRepository userRepository)
        {
            _chatEventService = chatEventService;
            _groupRepository = groupRepository;
            _chatRepository = chatRepository;
            _chatUserRepository = chatUserRepository;
            _userRepository = userRepository;
        }

        public override void Seed()
        {
            if (_chatRepository.Any() || !_userRepository.Any())
            {
                return;
            }
            
            var chats = new[]
            {
                new Chat()
                {
                    Id = Guid.NewGuid(),
                    GroupId = new Guid("c49ff16c-842c-4c13-853c-acea6ee4d28d")
                },
                new Chat()
                {
                    Id = Guid.NewGuid(),
                    GroupId = new Guid("8f10e5e0-02f6-47cc-84c7-cd4e5b06792f")
                },
                new Chat()
                {
                    Id = Guid.NewGuid(),
                    GroupId = new Guid("3348dce8-26f0-4da5-b683-f0dedb462d62")
                }
            };

            var users = _userRepository.GetAll();
            
            foreach(var user in users) 
            {
                foreach (var chat in chats)
                {
                    _chatUserRepository.Add(new ChatUser(user, chat));
                }
            }
            
            var author = users.First();
            
            foreach (var chat in chats)
            {
                var group = _groupRepository.Get(chat.GroupId);
                _chatEventService.AddChatCreatedEvent(group, author, chat);
            }
        }
        
        public override int GetPriority()
        {
            return 3;
        }
    }
}
