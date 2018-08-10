using System;
using System.Collections.Generic;
using CallGate.Repositories;
using AutoMapper;
using CallGate.ApiModels.Group;
using CallGate.Models;
using CallGate.Services.Email;
using CallGate.Services.Helper;
using CallGate.Stores;

namespace CallGate.Services.Group
{
    public class GroupService : IGroupService
    {
        private readonly IAuthorizedUserHelper _authorizedUserHelper;
        private readonly IGroupUserRepository _groupUserRepository;
        private readonly IGroupUserStore _groupUserStore;
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupEventService _groupEventService;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        private readonly Models.User _user;

        public GroupService(
            IAuthorizedUserHelper authorizedUserHelper, 
            IGroupUserRepository groupUserRepository,
            IGroupUserStore groupUserStore,
            IGroupRepository groupRepository, 
            IGroupEventService groupEventService,
            IMailService mailService,
            IMapper mapper
        )
        {
            _authorizedUserHelper = authorizedUserHelper;
            _groupUserRepository = groupUserRepository;
            _groupUserStore = groupUserStore;
            _groupRepository = groupRepository;
            _groupEventService = groupEventService;
            _mailService = mailService;
            _mapper = mapper;
            _user = authorizedUserHelper.GetAuthorizedUser();
        }

        public IEnumerable<GroupResponse> GetAll()
        {
            var groups = _groupRepository.GetAllByUserId(_user.Id);

            return _mapper.Map<IEnumerable<GroupResponse>>(groups);
        }

        public GroupResponse Get(Guid groupId)
        {
            var group = _groupRepository.GetByUserId(groupId, _user.Id);

            return _mapper.Map<GroupResponse>(group);
        }

        public GroupResponse GetOneByName(string name)
        {
            var group = _groupRepository.GetOneByName(name);

            return _mapper.Map<GroupResponse>(group);
        }

        public GroupResponse Create(string name, string description)
        {
            var group = new Models.Group
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description
            };
                
            var user = _authorizedUserHelper.GetAuthorizedUser();
            var groupUser = new GroupUser(user, group, Role.Admin);
            var documentGroupUser = new Documents.GroupUser(user.Id, group.Id);
            
            _groupUserRepository.Add(groupUser);
            _groupUserStore.AddToBus(documentGroupUser);
            
            _groupEventService.AddGroupCreatedEvent(group, _user);
            _groupEventService.AddUserAddedToGroupEvent(group, _user, _user);
            
            _mailService.SendMail(
                user.Email, 
                "CallGate - New group created successfully", 
                $"Congratz! You created a new group '{name}'."
            );
            
            return _mapper.Map<GroupResponse>(group);
        }

        public void Edit(Guid groupId, string name, string description)
        {
            var group = _groupRepository.GetByRoleAndUserId(groupId, Role.Admin, _user.Id);
            
            if (name != null)
            {
                group.Name = name;
            }

            if (description != null)
            {
                group.Description = description;
            }

            _groupEventService.AddGroupEditedEvent(group, _user);
        }
    }
}