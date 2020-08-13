using System.Collections.Generic;
using Newtonsoft.Json;

namespace DebugVisualizer.DataExtraction.Data
{
    public sealed class ListData : VisualizationData
    {
        public override string[] Tags => new string[] {"grid"};
        
        [JsonProperty("items")] public List<Dictionary<string, object>> Items { get; set; }

        public ListData() : this(new List<Dictionary<string, object>>())
        {
        }

        public ListData(List<Dictionary<string, object>> items)
        {
            this.Items = items;
        }
    }
}