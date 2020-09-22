using System.Collections.Generic;
using Newtonsoft.Json;

namespace DebugVisualizer.Brokerage.Data
{
    public sealed class TableData : VisualizationData
    {
        public override string[] Tags => new[] {"table"};

        [JsonProperty("rows")] public List<Dictionary<string, object>> Items { get; set; }

        public TableData() : this(new List<Dictionary<string, object>>())
        {
        }

        public TableData(List<Dictionary<string, object>> items)
        {
            this.Items = items;
        }
    }
}