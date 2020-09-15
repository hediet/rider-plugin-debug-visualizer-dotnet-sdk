using System;
using DebugVisualizer.Brokerage.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DebugVisualizer.Brokerage.Brokers
{
    public class JsonVisualizationBroker : GenericVisualizationBroker<string>
    {
        public override void GetExtractions(string value, IVisualizationBrokerContext context)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<JObject>(value);
                var data = JsonVisualizationData.From(obj);
                context.Add(
                    () => data,
                    new VisualizationBrokerInfo("JsonData", "JSON Data", 1000)
                );
            }
            catch (Exception)
            {
                // Don't report anything if an exception is thrown
            }
        }
    }
}