using System;

namespace CallGate.ApiModels.Authentication
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
