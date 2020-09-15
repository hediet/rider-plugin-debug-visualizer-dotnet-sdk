using Newtonsoft.Json;

namespace DebugVisualizer.Brokerage.Data
{
    public sealed class TextData : VisualizationData
    {
        public override string[] Tags => new string[] { "text" };

        [JsonProperty("text")]
        public string TextValue { get; set; }

        public TextData(string text)
        {
            this.TextValue = text;
        }
    }
}