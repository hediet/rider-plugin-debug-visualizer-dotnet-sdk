using Newtonsoft.Json;

namespace DebugVisualizer.DataExtraction
{
    public sealed class DataExtractionOptions
    {
        public DataExtractionOptions(string? preferredDataExtractorId)
        {
            PreferredDataExtractorId = preferredDataExtractorId;
        }
        
        [JsonProperty("preferredDataExtractorId")]
        public string? PreferredDataExtractorId { get; }
    }
}