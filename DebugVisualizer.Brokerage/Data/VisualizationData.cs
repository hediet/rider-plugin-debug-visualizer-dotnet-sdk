using System.Collections.Generic;
using System.Linq;
using DebugVisualizer.Brokerage.Brokers;
using Newtonsoft.Json;

namespace DebugVisualizer.Brokerage.Data
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public abstract class VisualizationData
    {
        [JsonProperty("kind")]
        public Dictionary<string, bool> Kind
        {
            get { return Tags.ToDictionary(tag => tag, tag => true); }
        }

        [JsonIgnore] public abstract string[] Tags { get; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}