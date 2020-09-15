using System.Collections.Generic;
using DebugVisualizer.Brokerage.Data;

namespace DebugVisualizer.Brokerage.Brokers
{
    class ToStringVisualizationBroker : IVisualizationBroker
    {
        public void Broker(object? value, IVisualizationBrokerContext context)
        {
            context.Add(
                () => new TextData(value?.ToString() ?? "(null)"),
                new VisualizationBrokerInfo("ToString", "ToString", 100)
            );
        }
    }
}