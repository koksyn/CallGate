using System;
using System.Net;

namespace CallGate.Exceptions
{
    public class ParseApiException : Exception, IApiException
    {
        private readonly string _message;
        
        public ParseApiException(string message = "Validation failed.") : base(message)
        {
            _message = message;
        }

        public string GetMessage()
        {
            return _message;
        }

        public int GetHttpStatusCode()
        {
            return (int) HttpStatusCode.BadRequest;
        }
    }
}