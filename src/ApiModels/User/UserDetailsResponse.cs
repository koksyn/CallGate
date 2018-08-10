using System;

namespace CallGate.ApiModels.User
{
    public class UserDetailsResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public DateTime JoinedGroupAt { get; set; }
    }
}