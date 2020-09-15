using System.Collections.Generic;
using DebugVisualizer.Brokerage.Data;

namespace DebugVisualizer.Brokerage.Brokers
{
    class VisualizationDataVisualizationBroker : GenericVisualizationBroker<VisualizationData>
    {
        public override void GetExtractions(VisualizationData value, IVisualizationBrokerContext context)
        {
            context.Add(
                () => value,
                new VisualizationBrokerInfo("VisualizationDataExtractor", "Visualization", 1000)
            );
        }
    }
}