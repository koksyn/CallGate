using System;
using System.Linq;
using CallGate.Documents;
using CallGate.Models;
using CallGate.Repositories;
using CallGate.Stores;

namespace CallGate.Seeders
{
    public class MessageSeeder : BaseSeeder
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMessageStore _messageStore;
        private readonly Random _random;

        public MessageSeeder(
            IChannelRepository channelRepository,
            IChatRepository chatRepository, 
            IUserRepository userRepository,
            IMessageStore messageStore)
        {
            _channelRepository = channelRepository;
            _chatRepository = chatRepository;
            _userRepository = userRepository;
            _messageStore = messageStore;
            _random = new Random();
        }

        public override void Seed()
        {
            if (_messageStore.Any() || !_userRepository.Any())
            {
                return;
            }

            var user = _userRepository.GetAll().First();
            
            SeedChatMessages(user);
            SeedChannelMessages(user);
        }
        
        private void SeedChatMessages(User user)
        {
            if (_chatRepository.Any())
            {
                var chats = _chatRepository.GetAll();

                foreach (var chat in chats)
                {
                    var message = GenerateRandomMessage(user);
                    message.ChatId = chat.Id;
                    
                    _messageStore.Add(message);
                }
            }
        }

        private void SeedChannelMessages(User user)
        {
            if (_channelRepository.Any())
            {
                var channels = _channelRepository.GetAll();

                foreach (var channel in channels)
                {
                    var message = GenerateRandomMessage(user);
                    message.ChannelId = channel.Id;
                    
                    _messageStore.Add(message);
                }
            }
        }
        
        private static readonly string[] RandomContents =
        {
            "Hey!", "Cool", "Hello", "Hi there!", "Can I ask you something?"
        };

        private Message GenerateRandomMessage(User user)
        {
            return new Message
            {
                Id = Guid.NewGuid(),
                Content =  RandomContents[_random.Next(RandomContents.Length)],
                Created = DateTime.Now,
                UserId = user.Id,
                Username = user.Username
            };
        }
        
        public override int GetPriority()
        {
            return 5;
        }
    }
}