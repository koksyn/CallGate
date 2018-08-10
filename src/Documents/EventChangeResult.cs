using Newtonsoft.Json;

namespace CallGate.Documents
{
    public class EventChangeResult
    {
        [JsonProperty("new_val", NullValueHandling = NullValueHandling.Ignore)]
        public Event NewValue { get; set; }
        
        [JsonProperty("old_val", NullValueHandling = NullValueHandling.Ignore)]
        public Event OldValue { get; set; }
    }
}