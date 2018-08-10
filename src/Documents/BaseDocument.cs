using System;
using Newtonsoft.Json;

namespace CallGate.Documents
{
    public class BaseDocument : IDocument
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public Guid Id { get; set; }

        public Guid GetId()
        {
            return Id;
        }
    }
}