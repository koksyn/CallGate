using System;
using System.Linq;
using CallGate.Models;
using CallGate.Repositories;
using CallGate.Services.Group;
using CallGate.Stores;

namespace CallGate.Seeders
{
    public class GroupSeeder : BaseSeeder
    {
        private readonly IGroupUserStore _groupUserStore;
        private readonly IGroupUserRepository _groupUserRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGroupEventService _groupEventService;

        public GroupSeeder(
            IGroupUserStore groupUserStore,
            IGroupUserRepository groupUserRepository,
            IGroupRepository groupRepository,
            IUserRepository userRepository,
            IGroupEventService groupEventService)
        {
            _groupUserStore = groupUserStore;
            _groupUserRepository = groupUserRepository;
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            _groupEventService = groupEventService;
        }

        public override void Seed()
        {
            if (_groupRepository.Any())
            {
                return;
            }

            var groups = new[]
            {
                new Group()
                {
                    Id = new Guid("c49ff16c-842c-4c13-853c-acea6ee4d28d"), 
                    Name = ".NET Developers group",
                    Description = "Backend services, complex businness logic, network communication, all these things are here."
                },
                new Group()
                {
                    Id = new Guid("3348dce8-26f0-4da5-b683-f0dedb462d62"),
                    Name = "Web developers group",
                    Description = "Share knowledge, passion and tech news. All web developers are welcome."
                },
                new Group()
                {
                    Id = new Guid("8f10e5e0-02f6-47cc-84c7-cd4e5b06792f"),
                    Name = "Unity 3D world",
                    Description = "This group is about developing and programming Games on Unity platform."
                }
            };

            if (!_userRepository.Any())
            {
                return;
            }

            var users = _userRepository.GetAll();
            var author = users.First();
            
            SeedGroupEvents(author, groups);

            foreach (var user in users)
            {
                SeedGroupUsers(user, groups);
                SeedDocumentGroupUsers(user, groups);
                SeedGroupUserEvents(groups, author, user);
            }
        }
        
        private void SeedGroupEvents(User author, Group[] groups)
        {
            foreach (var group in groups)
            {
                _groupEventService.AddGroupCreatedEvent(group, author);
            }
        }
        
        private void SeedGroupUsers(User user, Group[] groups)
        {
            _groupUserRepository.Add(new GroupUser(user, groups[0], Role.Admin));
            _groupUserRepository.Add(new GroupUser(user, groups[1]));
            _groupUserRepository.Add(new GroupUser(user, groups[2]));
        }
        
        private void SeedDocumentGroupUsers(User user, Group[] groups)
        {
            foreach (var group in groups)
            {
                _groupUserStore.Add(new Documents.GroupUser(user.Id, group.Id));
            }
        }
        
        private void SeedGroupUserEvents(Group[] groups, User author, User user)
        {
            foreach (var group in groups)
            {
                _groupEventService.AddUserAddedToGroupEvent(group, author, user);
            }
        }

        public override int GetPriority()
        {
            return 2;
        }
    }
}
