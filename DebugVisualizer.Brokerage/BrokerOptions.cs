using Newtonsoft.Json;

namespace DebugVisualizer.Brokerage
{
    public sealed class BrokerOptions
    {
        public BrokerOptions(string? preferredBrokerId)
        {
            PreferredBrokerId = preferredBrokerId;
        }
        
        [JsonProperty("preferredBrokerId")]
        public string? PreferredBrokerId { get; }
    }
}