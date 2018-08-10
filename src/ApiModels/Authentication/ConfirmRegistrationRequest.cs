using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CallGate.ApiModels.Authentication
{
    public class ConfirmRegistrationRequest
    {
        [Required]
        [FromForm(Name = "confirmationPhrase")]
        public string ConfirmationPhrase { get; set; }
    }
}
