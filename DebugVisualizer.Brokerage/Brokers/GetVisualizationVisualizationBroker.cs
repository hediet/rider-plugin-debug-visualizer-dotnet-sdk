using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using DebugVisualizer.Brokerage.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DebugVisualizer.Brokerage.Brokers
{
    public interface IVisualizable
    {
        public object GetVisualizationData();
    }

    class GetVisualizationVisualizationBroker : IVisualizationBroker
    {
        public void Broker(object? value, IVisualizationBrokerContext context)
        {
            if (value == null)
            {
                return;
            }

            var type = value.GetType();
            var m = type.GetMethod("GetVisualizationData",
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (m == null)
            {
                return;
            }

            object? result;
            try
            {
                result = m.Invoke(value, null);
            }
            catch (Exception e)
            {
                return;
            }

            foreach (var extraction in context.Broker(result))
            {
                context.Add(
                    extraction.GetVisualizationData,
                    new VisualizationBrokerInfo($"GetVisualizationData-{extraction.BrokerInfo.Id}",
                        $"GetVisualizationData ({extraction.BrokerInfo.Name})",
                        1000 + extraction.BrokerInfo.Priority)
                );
            }
        }
    }
}