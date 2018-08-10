using System.Linq;
using CallGate.Documents;
using CallGate.Repositories;
using CallGate.Services.Channel;
using CallGate.Stores;

namespace CallGate.Seeders
{
    public class ChannelUserSeeder : BaseSeeder
    {
        private readonly IChannelEventService _channelEventService;
        private readonly IChannelUserStore _channelUserStore;
        private readonly IChannelUserRepository _channelUserRepository;
        private readonly IGroupRepository _groupRepository;

        public ChannelUserSeeder(
            IChannelEventService channelEventService,
            IChannelUserStore channelUserStore,
            IChannelUserRepository channelUserRepository, 
            IGroupRepository groupRepository)
        {
            _channelEventService = channelEventService;
            _channelUserStore = channelUserStore;
            _channelUserRepository = channelUserRepository;
            _groupRepository = groupRepository;
        }

        public override void Seed()
        {
            if (_channelUserStore.Any() || !_channelUserRepository.Any())
            {
                return;
            }

            var channelUsers = _channelUserRepository.GetAll();
            var author = channelUsers.First().User;

            foreach(var channelUser in channelUsers)
            {
                _channelUserStore.Add(new ChannelUser(channelUser.UserId, channelUser.ChannelId));
                
                var group = _groupRepository.Get(channelUser.Channel.GroupId);
                _channelEventService.AddUserAddedToChannelEvent(group, author, channelUser.User, channelUser.Channel);
            }
        }
        
        public override int GetPriority()
        {
            return 7;
        }
    }
}