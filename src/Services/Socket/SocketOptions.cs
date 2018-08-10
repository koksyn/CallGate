using System;
using Microsoft.AspNetCore.Builder;

namespace CallGate.Services.Socket
{
    public class SocketOptions : ISocketOptions
    {
        private readonly WebSocketOptions _options;
        
        public SocketOptions()
        {
            _options = new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromMinutes(2.0),
                ReceiveBufferSize = 4096
            };
        }

        public int GetBufferSize()
        {
            return _options.ReceiveBufferSize;
        }

        public WebSocketOptions GetOptions()
        {
            return _options;
        }
    }
}