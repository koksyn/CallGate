using System;
using System.ComponentModel.DataAnnotations;

namespace CallGate.ApiModels.Group
{
    public class GroupRequest
    {
        [Required]
        public Guid GroupId { get; set; }
    }
}