using System.Collections.Generic;
using DebugVisualizer.DataExtraction.Data;
using Newtonsoft.Json;

namespace DebugVisualizer.DataExtraction
{
    public sealed class DataExtractionResult
    {
        public static DataExtractionResult FromVisualizationData(VisualizationData data)
        {
            return new DataExtractionResult(new ExtractedData("default", data),
                new[] {new DataExtractorInfo("default", "Default", 0),});
        }

        public DataExtractionResult(ExtractedData? extractedData,
            IReadOnlyList<DataExtractorInfo> availableDataExtractors)
        {
            ExtractedData = extractedData;
            AvailableDataExtractors = availableDataExtractors;
        }

        [JsonProperty("extractedData")] public ExtractedData? ExtractedData { get; }


        [JsonProperty("availableDataExtractors")]
        public IReadOnlyList<DataExtractorInfo> AvailableDataExtractors { get; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public sealed class ExtractedData
    {
        public ExtractedData(string dataExtractorId, VisualizationData data)
        {
            DataExtractorId = dataExtractorId;
            Data = data;
        }

        [JsonProperty("dataExtractorId")] public string DataExtractorId { get; }
        [JsonProperty("data")] public VisualizationData Data { get; }
    }

    public sealed class DataExtractorInfo
    {
        public DataExtractorInfo(string id, string name, int priority)
        {
            Id = id;
            Name = name;
            Priority = priority;
        }

        [JsonProperty("id")] public string Id { get; }
        [JsonProperty("name")] public string Name { get; }
        [JsonProperty("priority")] public int Priority { get; }
    }
}