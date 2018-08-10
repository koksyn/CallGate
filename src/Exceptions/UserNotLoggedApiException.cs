using System;
using System.Net;

namespace CallGate.Exceptions
{
    public class UserNotLoggedApiException : Exception, IApiException
    {
        private readonly string _message;
        
        public UserNotLoggedApiException(string message = "Permission denied. You are not logged in.") : base(message)
        {
            _message = message;
        }

        public string GetMessage()
        {
            return _message;
        }

        public int GetHttpStatusCode()
        {
            return (int) HttpStatusCode.Forbidden;
        }
    }
}