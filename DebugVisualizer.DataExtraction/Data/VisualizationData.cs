using System.Collections.Generic;
using System.Linq;
using DebugVisualizer.DataExtraction.Extractors;
using Newtonsoft.Json;

namespace DebugVisualizer.DataExtraction.Data
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