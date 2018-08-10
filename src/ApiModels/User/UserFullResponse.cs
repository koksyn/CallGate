using System;

namespace CallGate.ApiModels.User
{
    public class UserFullResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}