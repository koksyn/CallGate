using System;
using System.Net;

namespace CallGate.Exceptions
{
    public class NotFoundApiException : Exception, IApiException
    {
        private new const string Message = "{0} was not found.";
        private readonly string _message;
        
        public NotFoundApiException(string objectName = "Object")
        {
            _message = string.Format(Message, objectName);
        }

        public string GetMessage()
        {
            return _message;
        }

        public int GetHttpStatusCode()
        {
            return (int) HttpStatusCode.NotFound;
        }
    }
}