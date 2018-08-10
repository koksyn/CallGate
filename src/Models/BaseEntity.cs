using System;

namespace CallGate.Models
{
    public class BaseEntity : IEntity
    {
        public Guid Id { get; set; }
    }
}
