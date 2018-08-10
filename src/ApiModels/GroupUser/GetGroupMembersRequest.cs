using System.ComponentModel.DataAnnotations;
using CallGate.ApiModels.Group;
using Microsoft.AspNetCore.Mvc;

namespace CallGate.ApiModels.GroupUser
{
    public class GetGroupMembersRequest : GroupRequest
    {
        [FromQuery(Name = "username")]
        [StringLength(255)]
        public string Username { get; set; }   
    }
}