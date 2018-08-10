using System.ComponentModel.DataAnnotations;
using CallGate.ApiModels.Group;
using CallGate.Models;

namespace CallGate.ApiModels.GroupUser
{
    public class EditGroupUserRequest : GroupRequest
    {
        [Required]
        [StringLength(255)]
        public string Username { get; set; }
        
        [Required]
        public Role Role { get; set; }
    }
}