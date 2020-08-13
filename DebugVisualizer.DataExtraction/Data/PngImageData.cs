using Newtonsoft.Json;

namespace DebugVisualizer.DataExtraction.Data
{
    public sealed class PngImageData : VisualizationData
    {
        public override string[] Tags => new string[] { "imagePng" };

        [JsonProperty("base64Data")]
        public string Base64Data { get; set; }

        public PngImageData(string base64Data)
        {
            Base64Data = base64Data;
        }
    }
}