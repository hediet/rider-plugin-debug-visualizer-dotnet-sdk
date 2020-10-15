using System;
using System.Collections.Generic;
using System.Linq;
using DebugVisualizer.Brokerage.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DebugVisualizer.Brokerage.Brokers
{
    public class NumberArrayVisualizationBroker : IVisualizationBroker
    {
        public void Broker(object? value, IVisualizationBrokerContext context)
        {
            if (value is IEnumerable<int> intItems)
            {
                context.Add(() => PlotlyData.From(intItems.Select(i => (double) i)),
                    new VisualizationBrokerInfo("plot-int-array", "Plot Int Array", 1000));
            }

            if (value is IEnumerable<double> doubleItems)
            {
                context.Add(() => PlotlyData.From(doubleItems.Select(i => i)),
                    new VisualizationBrokerInfo("plot-double-array", "Plot Double Array", 1000));
            }

            if (value is IEnumerable<float> floatItems)
            {
                context.Add(() => PlotlyData.From(floatItems.Select(i => (double) i)),
                    new VisualizationBrokerInfo("plot-float-array", "Plot Float Array", 1000));
            }

            if (value is IEnumerable<decimal> decimalItems)
            {
                context.Add(() => PlotlyData.From(decimalItems.Select(i => (double) i)),
                    new VisualizationBrokerInfo("plot-decimal-array", "Plot Decimal Array", 1000));
            }
        }
    }
}