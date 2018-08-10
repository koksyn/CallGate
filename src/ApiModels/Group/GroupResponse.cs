using System;

namespace CallGate.ApiModels.Group
{
    public class GroupResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}