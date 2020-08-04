using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace DebugVisualizer.DataExtraction.Data
{
    public sealed class PlotlyData : VisualizationData
    {
        public override string[] Tags => new string[] { "plotly" };

        [JsonProperty("data")]
        public List<PlotlyPlotData> Data { get; set; }

        public PlotlyData(IEnumerable<PlotlyPlotData> data = null)
        {
            this.Data = data.ToList();
        }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class PlotlyPlotData
    {
        [JsonProperty("x")]
        public List<double>? X { get; set; }
        
        [JsonProperty("y")]
        public List<double>? Y { get; set; }
    }
}