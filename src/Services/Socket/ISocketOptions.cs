using CallGate.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace CallGate.Services.Socket
{
    public interface ISocketOptions : IScopedDependency
    {
        int GetBufferSize();
        WebSocketOptions GetOptions();
    }
}