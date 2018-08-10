using System.ComponentModel.DataAnnotations;

namespace CallGate.ApiModels.Group
{
    public class GroupCreateRequest
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(768)]
        public string Description { get; set; }   
    }
}