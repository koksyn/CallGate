using CallGate.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CallGate.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is IApiException)
            {
                IApiException exception = (IApiException) context.Exception;
                
                var content = new { Error = exception.GetMessage() };
                var result = new JsonResult(content);
                
                result.StatusCode = exception.GetHttpStatusCode();
                context.Result = result;
            }
        }
    }
}