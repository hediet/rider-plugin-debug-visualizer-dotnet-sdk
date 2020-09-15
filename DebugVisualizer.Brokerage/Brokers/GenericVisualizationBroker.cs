using System.Collections.Generic;

namespace DebugVisualizer.Brokerage.Brokers
{
    public abstract class GenericVisualizationBroker<T> : IVisualizationBroker
    {
        public void Broker(object? value, IVisualizationBrokerContext context)
        {
            if (value is T val)
            {
                GetExtractions(val, context);
            }
        }

        public abstract void GetExtractions(T value, IVisualizationBrokerContext context);
    }
}