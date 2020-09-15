using System.Collections.Generic;
using DebugVisualizer.Brokerage.Data;
using Newtonsoft.Json;

namespace DebugVisualizer.Brokerage
{
    public sealed class VisualizationDataWithBrokers
    {
        public static VisualizationDataWithBrokers FromVisualizationData(VisualizationData data)
        {
            return new VisualizationDataWithBrokers(new VisualizationDataWithBroker(data, "default"),
                new[] {new VisualizationBrokerInfo("default", "Default", 0),});
        }

        public VisualizationDataWithBrokers(VisualizationDataWithBroker? data,
            IReadOnlyList<VisualizationBrokerInfo> availableBrokers)
        {
            Data = data;
            AvailableBrokers = availableBrokers;
        }

        [JsonProperty("data")] public VisualizationDataWithBroker? Data { get; }


        [JsonProperty("availableBrokers")]
        public IReadOnlyList<VisualizationBrokerInfo> AvailableBrokers { get; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public sealed class VisualizationDataWithBroker
    {
        public VisualizationDataWithBroker(VisualizationData data, string brokerId)
        {
            BrokerId = brokerId;
            Data = data;
        }

        [JsonProperty("brokerId")] public string BrokerId { get; }
        [JsonProperty("data")] public VisualizationData Data { get; }
    }

    public sealed class VisualizationBrokerInfo
    {
        public VisualizationBrokerInfo(string id, string name, int priority)
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