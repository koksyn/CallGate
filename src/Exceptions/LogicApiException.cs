using System;
using System.Net;

namespace CallGate.Exceptions
{
    public class LogicApiException : Exception, IApiException
    {
        private readonly string _message;
        
        public LogicApiException(string message = "Conflict") : base(message)
        {
            _message = message;
        }

        public string GetMessage()
        {
            return _message;
        }

        public int GetHttpStatusCode()
        {
            return (int) HttpStatusCode.Conflict;
        }
    }
}