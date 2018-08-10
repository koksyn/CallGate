using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CallGate.ApiModels.Socket;
using CallGate.Services.Utils;
using Newtonsoft.Json;

namespace CallGate.Services.Socket
{
    public class SocketStream : ISocketStream
    {
        private readonly IJsonSerializer _jsonSerializer;
        private CancellationToken _cancellationToken;
        private readonly byte[] _buffer;

        public SocketStream(
            ISocketOptions socketOptions,
            IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
            var bufferSize = socketOptions.GetBufferSize();
            _buffer = new byte[bufferSize];
            _cancellationToken = default(CancellationToken);
        }
        
        public async Task<SocketMessage<MessageRequest>> ReceiveAsync(WebSocket webSocket)
        {
            /*
             * Message can be sent by chunk.
             * We must read all chunks before decoding the content
             */
            var segment = new ArraySegment<byte>(_buffer);
            var message = new SocketMessage<MessageRequest>();
            
            using (var stream = new MemoryStream())
            {
                WebSocketReceiveResult result;
                
                do
                {
                    _cancellationToken.ThrowIfCancellationRequested();

                    result = await webSocket.ReceiveAsync(segment, _cancellationToken);

                    if (result.CloseStatus.HasValue)
                    {
                        break;
                    }
                    
                    if (result.MessageType != WebSocketMessageType.Text)
                    {
                        throw new Exception("Unexpected message");
                    }
                    
                    stream.Write(segment.Array, segment.Offset, result.Count);
                }
                while (!result.EndOfMessage);
                
                message.Result = result;
                message.Raw = await ToRawString(stream);
            }
            
            return message;
        }
        
        public async Task SendObjectAsync(WebSocket webSocket, object obj)
        {
            var serialized = _jsonSerializer.Serialize(obj);
            
            await SendStringAsync(webSocket, serialized);
        }
        
        public async Task SendStringAsync(WebSocket webSocket, string rawMessage)
        {
            var buffer = Encoding.UTF8.GetBytes(rawMessage);
            var segment = new ArraySegment<byte>(buffer);

            await webSocket.SendAsync(
                segment, 
                WebSocketMessageType.Text, 
                true, 
                _cancellationToken
            );
        }

        private static async Task<string> ToRawString(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            
            // Encoding UTF8: https://tools.ietf.org/html/rfc6455#section-5.6
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}