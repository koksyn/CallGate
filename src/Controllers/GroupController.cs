using System;
using CallGate.ApiModels.Group;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CallGate.Filters;
using CallGate.Filters.NotReusable;
using CallGate.Models;
using CallGate.Services.Group;

namespace CallGate.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var groups = _groupService.GetAll();

            return Json(groups);
        }
        
        [HttpGet("roles")]
        public IActionResult GetRolesForAnyGroup()
        {
            var roles = Enum.GetNames(typeof(Role));
            
            return Json(roles);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var group = _groupService.Get(id);

            if(group == null)
            {
                return NotFound();
            }

            return Json(group);
        }
        
        [HttpPost]
        [ApiModelValidationFilter]
        [ServiceFilter(typeof(TransactionFilter))]
        public IActionResult Create(GroupCreateRequest command)
        {   
            var group = _groupService.Create(command.Name, command.Description);
           
            return Created($"/api/group/{@group.Id}", null);
        }
        
        [HttpPatch("{groupId}")]
        [ApiModelValidationFilter]
        [ServiceFilter(typeof(GroupAdminFilter))]
        [ServiceFilter(typeof(TransactionFilter))]
        public IActionResult Edit(GroupEditRequest command)
        {
            _groupService.Edit(command.GroupId, command.Name, command.Description);
            
            return NoContent();
        }
    }
}