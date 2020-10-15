using System;
using System.Collections.Generic;
using System.Linq;
using DebugVisualizer.Brokerage.Data;
using Mono.Cecil;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DebugVisualizer.Brokerage.Brokers
{
    public class ClassHierarchyVisualizationBroker : IVisualizationBroker
    {
        public void Broker(object? value, IVisualizationBrokerContext context)
        {
            if (value == null)
            {
                return;
            }

            var type = value.GetType();
            if (type.IsGenericType)
            {
                type = type.GetGenericTypeDefinition();
            }

            var typeInfo = AssemblyDefinition.ReadAssembly(type.Assembly.Location).MainModule.GetType(type.FullName);
            if (typeInfo == null)
            {
                return;
            }
            context.Add(() =>
                    GraphData.From(new TypeReference[] {typeInfo}, (cur, nodeInfo) =>
                    {
                        if (cur == typeInfo)
                        {
                            nodeInfo.Color = "orange";
                        }
                        
                        nodeInfo.Label = cur.Name;
                        var def = cur.Resolve();
                        if (def == null)
                        {
                            return;
                        }

                        if (def.IsClass)
                        {
                            if (def.BaseType != null)
                            {
                                nodeInfo.Edges.Add(new GraphData.EdgeInfo(def.BaseType));
                            }

                            foreach (var i in def.Interfaces)
                            {
                                nodeInfo.Edges.Add(new GraphData.EdgeInfo(i.InterfaceType)
                                {
                                    Style = GraphData.EdgeStyle.Dashed
                                });
                            }
                        }
                        else if (def.IsInterface)
                        {
                            foreach (var i in def.Interfaces)
                            {
                                nodeInfo.Edges.Add(new GraphData.EdgeInfo(i.InterfaceType));
                            }
                        }
                        else
                        {
                            nodeInfo.Label = "unknown";
                        }
                    }),
                new VisualizationBrokerInfo("type-hierarchy", "Type Hierarchy", 50));
        }
    }
}