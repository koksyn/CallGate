using System.ComponentModel.DataAnnotations;

namespace CallGate.ApiModels.Authentication
{
    public class RegisterRequest
    {
        [Required]
        [StringLength(255)]
        public string Username { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Password { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [StringLength(512)]
        public string RedirectUrl { get; set; }
    }
}
