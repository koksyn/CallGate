using System;
using System.Net;

namespace CallGate.Exceptions
{
    public class ResourceNotFoundApiException : Exception, IApiException
    {
        private new const string Message = "{0} with provided '{1}': '{2}' was not found.";
        private readonly string _message;
        
        public ResourceNotFoundApiException(string resourceId, string resourceName, string identifierName = "id")
        {
            _message = string.Format(Message, resourceName, identifierName, resourceId);
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