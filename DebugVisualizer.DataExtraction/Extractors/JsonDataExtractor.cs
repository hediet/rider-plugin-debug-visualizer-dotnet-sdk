using DebugVisualizer.DataExtraction.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DebugVisualizer.DataExtraction.Extractors
{
    public class JsonDataExtractor : GenericDataExtractor<string>
    {
        public override void GetExtractions(string value, IDataExtractorContext context)
        {
            var obj = JsonConvert.DeserializeObject<JObject>(value);
            var data = JsonVisualizationData.From(obj);

            context.AddExtraction(
                () => data,
                new DataExtractorInfo("JsonData", "JSON Data", 1000)
            );
        }
    }
}