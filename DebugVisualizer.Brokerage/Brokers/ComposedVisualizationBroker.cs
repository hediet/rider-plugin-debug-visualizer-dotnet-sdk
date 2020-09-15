using System.Collections.Generic;
using System.Linq;

namespace DebugVisualizer.Brokerage.Brokers
{
    public sealed class ComposedVisualizationBroker : IVisualizationBroker
    {
        public List<IVisualizationBroker> DataExtractors { get; }
        
        public ComposedVisualizationBroker(List<IVisualizationBroker> dataExtractors)
        {
            DataExtractors = dataExtractors;
        }

        public void Broker(object? value, IVisualizationBrokerContext context)
        {
            foreach (var d in DataExtractors)
            {
                d.Broker(value, context);
            }
        }
    }
}