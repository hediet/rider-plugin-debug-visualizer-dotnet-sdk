using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DebugVisualizer.Brokerage.Data;

namespace DebugVisualizer.Brokerage.Brokers
{
    public class TabularDataVisualizationBroker : GenericVisualizationBroker<IEnumerable<object>>
    {
        public override void GetExtractions(IEnumerable<object> value, IVisualizationBrokerContext context)
        {
            context.Add(() =>
            {
                var data = new TableData();

                foreach (var item in value.Take(10000))
                {
                    var dict = new Dictionary<string, object>();
                    data.Items.Add(dict);

                    foreach (var m in item.GetType()
                        .GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                        .OfType<PropertyInfo>())
                    {
                        var val = m.GetValue(item);
                        if (val != null)
                        {
                            dict[m.Name] = val;
                        }
                    }
                }

                return data;
            }, new VisualizationBrokerInfo("table", "Table", 200));
        }
    }
}