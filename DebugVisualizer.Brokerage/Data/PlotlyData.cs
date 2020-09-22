using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DebugVisualizer.Brokerage.Data
{
    public sealed class PlotlyData : VisualizationData
    {
        public override string[] Tags => new string[] { "plotly" };

        [JsonProperty("data")]
        public List<PlotlyPlotData> Data { get; set; }

        public PlotlyData(IEnumerable<PlotlyPlotData>? data = null)
        {
            Data = data != null ? data.ToList() : new List<PlotlyPlotData>();
        }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class PlotlyPlotData
    {
        [JsonProperty("opacity")]
        public double? Opacity { get; set; } 
        
        [JsonProperty("color")]
        public string? Color { get; set; } 
        
        [JsonProperty("type")]
        public PlotType? Type { get; set; } 
        
        [JsonProperty("x")]
        public List<double>? X { get; set; }
        
        [JsonProperty("y")]
        public List<double>? Y { get; set; }
        
        [JsonProperty("z")]
        public List<double>? Z { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PlotType
    {
        [EnumMember(Value = "mesh3d")]
        Mesh3D
    }
}