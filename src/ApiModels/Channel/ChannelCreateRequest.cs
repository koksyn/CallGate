using System.ComponentModel.DataAnnotations;
using CallGate.ApiModels.Group;

namespace CallGate.ApiModels.Channel
{
    public class ChannelCreateRequest : GroupRequest
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
    }
}