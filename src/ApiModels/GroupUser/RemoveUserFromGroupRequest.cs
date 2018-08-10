using System.ComponentModel.DataAnnotations;
using CallGate.ApiModels.Group;
using Microsoft.AspNetCore.Mvc;

namespace CallGate.ApiModels.GroupUser
{
    public class RemoveUserFromGroupRequest : GroupRequest
    {
        [Required]
        [StringLength(255)]
        [FromQuery(Name = "username")]
        public string Username { get; set; }
    }
}