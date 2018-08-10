using System;
using System.Net;

namespace CallGate.Exceptions
{
    public class NotEnoughRightsApiException : Exception, IApiException
    {
        private readonly string _message;
        
        public NotEnoughRightsApiException(string message = "Permission denied. You have not required rights to perform this action.") : base(message)
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