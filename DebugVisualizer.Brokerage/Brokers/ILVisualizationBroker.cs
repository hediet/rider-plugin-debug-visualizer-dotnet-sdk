using System;
using System.IO;
using System.Reflection;
using DebugVisualizer.Brokerage.Data;
using ILDisassembler;

namespace DebugVisualizer.Brokerage.Brokers
{
    public class ILVisualizationBroker : GenericVisualizationBroker<MethodBase>
    {
        public override void GetExtractions(MethodBase value, IVisualizationBrokerContext context)
        {
            var d = new Disassembler();

            context.Add(() => new TextData(d.DisassembleMethod(value)),
                new VisualizationBrokerInfo("il-method-body", "IL Method Body",1000));
        }
    }
}