using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CallGate.ApiModels.User
{
    public class RemoveAuthorizedUserRequest
    {
        [Required]
        [StringLength(255)]
        [FromQuery(Name = "password")]
        public string Password { get; set; }
    }
}
