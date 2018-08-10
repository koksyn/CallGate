using System;
using System.Threading.Tasks;
using CallGate.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace CallGate.Services.Socket
{
    public interface ISocketMiddleware : IScopedDependency
    {
        Task Invoke(HttpContext context, Func<Task> next);
    }
}