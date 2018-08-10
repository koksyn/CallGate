using System.ComponentModel.DataAnnotations;

namespace CallGate.ApiModels.Authentication
{
    public class LoginRequest
    {
        [Required]
        [StringLength(255)]
        public string Username { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Password { get; set; }
    }
}
