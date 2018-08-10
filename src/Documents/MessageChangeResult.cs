using Newtonsoft.Json;

namespace CallGate.Documents
{
    public class MessageChangeResult
    {
        [JsonProperty("new_val", NullValueHandling = NullValueHandling.Ignore)]
        public Message NewValue { get; set; }
        
        [JsonProperty("old_val", NullValueHandling = NullValueHandling.Ignore)]
        public Message OldValue { get; set; }
    }
}