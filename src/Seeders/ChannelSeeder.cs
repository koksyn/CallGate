using System;
using System.Linq;
using CallGate.Data;
using CallGate.Migrations;
using CallGate.Models;
using CallGate.Repositories;
using CallGate.Services.Channel;
using Microsoft.EntityFrameworkCore;

namespace CallGate.Seeders
{
    public class ChannelSeeder : BaseSeeder
    {
        private readonly IChannelEventService _channelEventService;
        private readonly IChannelUserRepository _channelUserRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;

        public ChannelSeeder(
            IChannelEventService channelEventService, 
            IChannelUserRepository channelUserRepository,
            IChannelRepository channelRepository, 
            IGroupRepository groupRepository,
            IUserRepository userRepository)
        {
            _channelEventService = channelEventService;
            _channelUserRepository = channelUserRepository;
            _channelRepository = channelRepository;
            _groupRepository = groupRepository;
            _userRepository = userRepository;
        }

        public override void Seed()
        {
            if (_channelRepository.Any() || !_userRepository.Any())
            {
                return;
            }
            
            var channels = new[]
            {
                new Channel()
                {
                    Id = Guid.NewGuid(),
                    GroupId = new Guid("c49ff16c-842c-4c13-853c-acea6ee4d28d"),
                    Name = "Offtopic"
                },
                new Channel()
                {
                    Id = Guid.NewGuid(),
                    GroupId = new Guid("c49ff16c-842c-4c13-853c-acea6ee4d28d"),
                    Name = "C# internals"
                },
                new Channel()
                {
                    Id = Guid.NewGuid(),
                    GroupId = new Guid("c49ff16c-842c-4c13-853c-acea6ee4d28d"),
                    Name = ".NET Core Framework"
                },
                new Channel()
                {
                    Id = Guid.NewGuid(),
                    GroupId = new Guid("8f10e5e0-02f6-47cc-84c7-cd4e5b06792f"),
                    Name = "3D modelling"
                },
                new Channel()
                {
                    Id = Guid.NewGuid(),
                    GroupId = new Guid("8f10e5e0-02f6-47cc-84c7-cd4e5b06792f"),
                    Name = "Unity jobs"
                },
                new Channel()
                {
                    Id = Guid.NewGuid(),
                    GroupId = new Guid("8f10e5e0-02f6-47cc-84c7-cd4e5b06792f"),
                    Name = "Tools"
                },
                new Channel()
                {
                    Id = Guid.NewGuid(),
                    GroupId = new Guid("8f10e5e0-02f6-47cc-84c7-cd4e5b06792f"),
                    Name = "Offtopic"
                },
                new Channel()
                {
                    Id = Guid.NewGuid(),
                    GroupId = new Guid("3348dce8-26f0-4da5-b683-f0dedb462d62"),
                    Name = "Frontend"
                },
                new Channel()
                {
                    Id = Guid.NewGuid(),
                    GroupId = new Guid("3348dce8-26f0-4da5-b683-f0dedb462d62"),
                    Name = "Backend"
                },
                new Channel()
                {
                    Id = Guid.NewGuid(),
                    GroupId = new Guid("3348dce8-26f0-4da5-b683-f0dedb462d62"),
                    Name = "DevOps / Serverless"
                }
            };

            var users = _userRepository.GetAll();

            foreach(var user in users) 
            {
                foreach (var channel in channels)
                {
                    _channelUserRepository.Add(new ChannelUser(user, channel));
                }
            }

            var author = users.First();
            
            foreach (var channel in channels)
            {
                var group = _groupRepository.Get(channel.GroupId);
                _channelEventService.AddChannelCreatedEvent(group, author, channel);
            }
        }
        
        public override int GetPriority()
        {
            return 4;
        }
    }
}