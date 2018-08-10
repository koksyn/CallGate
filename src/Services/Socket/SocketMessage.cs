using System;
using System.Net.WebSockets;
using Newtonsoft.Json;

namespace CallGate.Services.Socket
{
    public class SocketMessage<TObject> where TObject : new()
    {
        public WebSocketReceiveResult Result { get; set; }
        public string Raw { get; set; }

        public TObject GetObject()
        {
            return string.IsNullOrEmpty(Raw) ? new TObject() : DeserializeRawString(Raw);
        }

        private static TObject DeserializeRawString(string raw)
        {
            try
            {
                return JsonConvert.DeserializeObject<TObject>(raw);
            }
            catch (Exception exception)
            {
                throw new Exception($"Invalid JSON provided! Message: @{exception.Message}");    
            }
        }
    }
}