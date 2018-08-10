using System.ComponentModel.DataAnnotations;

namespace CallGate.ApiModels.Group
{
    public class GroupEditRequest : GroupRequest
    {
        [StringLength(255)]
        public string Name { get; set; }
        
        [StringLength(768)]
        public string Description { get; set; }   
    }
}