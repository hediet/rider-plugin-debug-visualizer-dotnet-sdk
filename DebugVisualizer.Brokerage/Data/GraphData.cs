using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DebugVisualizer.Brokerage.Data
{
    public sealed class GraphData : VisualizationData
    {
        public override string[] Tags => new string[] {"graph"};

        [JsonProperty("nodes")] public IList<NodeData> Nodes { get; set; } = new List<NodeData>();

        [JsonProperty("edges")] public IList<EdgeData> Edges { get; set; } = new List<EdgeData>();

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class NodeData
        {
            public NodeData(string id)
            {
                Id = id;
            }

            [JsonProperty("id")] public string Id { get; set; }

            [JsonProperty("label")] public string? Label { get; set; }

            [JsonProperty("color")] public string? Color { get; set; }

            [JsonProperty("shape")] public string? Shape { get; set; }
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class EdgeData
        {
            public EdgeData(string from, string to)
            {
                From = from;
                To = to;
            }

            [JsonProperty("from")] public string From { get; set; }

            [JsonProperty("to")] public string To { get; set; }

            [JsonProperty("label")] public string? Label { get; set; }

            [JsonProperty("id")] public string? Id { get; set; }

            [JsonProperty("color")] public string? Color { get; set; }

            [JsonProperty("style")] public EdgeStyle? Style { get; set; }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum EdgeStyle
        {
            [EnumMember(Value = "solid")] Solid,
            [EnumMember(Value = "dashed")] Dashed,
            [EnumMember(Value = "dotted")] Dotted
        }

        public static GraphData From<T>(IEnumerable<T> items, Action<T, NodeInfo> f) where T : notnull
        {
            var d = new GraphData();
            var q = new Queue<T>(items);
            var i = 0;
            var infos = new Dictionary<T, NodeInfo>();
            var processedItems = new HashSet<T>();

            string GetId(NodeInfo item)
            {
                if (item.Id == null)
                {
                    item.Id = "hediet.de/" + (i++);
                }

                return item.Id;
            }

            NodeInfo GetNodeInfo(T item)
            {
                if (infos.ContainsKey(item))
                    return infos[item];
                var info = new NodeInfo();
                f(item, info);
                infos.Add(item, info);
                return info;
            }

            while (q.Count > 0)
            {
                var item = q.Dequeue();
                if (!processedItems.Add(item))
                {
                    continue;
                }

                var nodeInfo = GetNodeInfo(item);
                var nd = new NodeData(GetId(nodeInfo));
                d.Nodes.Add(nd);

                nd.Label = nodeInfo.Label;
                nd.Color = nodeInfo.Color;

                foreach (var e in nodeInfo.Edges)
                {
                    if (!(e.To is T)) continue;

                    var ed = new EdgeData(nd.Id, GetId(GetNodeInfo((T) e.To)));
                    d.Edges.Add(ed);

                    ed.Label = e.Label;
                    ed.Id = e.Id;
                    ed.Style = e.Style;
                    ed.Color = e.Color;

                    q.Enqueue((T) e.To);
                }
            }

            return d;
        }

        public class NodeInfo
        {
            public IList<EdgeInfo> Edges { get; set; } = new List<EdgeInfo>();
            public string? Label { get; set; }
            public string? Id { get; set; }
            public string? Color { get; set; }

            public EdgeInfo AddEdge(object? to, string? id = null, string? label = null, string? color = null)
            {
                var e = new EdgeInfo(to) {Id = id, Label = label, Color = color};
                Edges.Add(e);
                return e;
            }
        }

        public class EdgeInfo
        {
            public object? To { get; set; }
            public string? Label { get; set; }
            public string? Id { get; set; }
            public string? Color { get; set; }

            public EdgeStyle? Style { get; set; }

            public EdgeInfo(object? to)
            {
                To = to;
            }
        }
    }

    public class GraphBuilder
    {
        private readonly List<object> _nodes = new List<object>();

        private readonly List<Action<object, GraphData.NodeInfo>> _closures =
            new List<Action<object, GraphData.NodeInfo>>();

        class Pointer
        {
            public string Name { get; }
            public object? Target { get; }

            public Pointer(string name, object? target)
            {
                Name = name;
                Target = target;
            }
        }

        public GraphBuilder WithHighlighting<T>(HashSet<T> highlightedValues, string highlightColor)
        {
            return WithClosure<T>((n, info) =>
            {
                if (highlightedValues.Contains(n))
                {
                    info.Color = highlightColor;
                }
            });
        }

        public GraphBuilder WithPointer(string name, object? target)
        {
            _nodes.Add(new Pointer(name, target));
            return this;
        }

        public GraphBuilder WithNode(object node)
        {
            _nodes.Add(node);
            return this;
        }

        public GraphBuilder WithClosure<T>(Action<T, GraphData.NodeInfo> f) where T : notnull
        {
            _closures.Add((value, info) =>
            {
                if (value is T t)
                {
                    f(t, info);
                }
            });
            return this;
        }

        public GraphData GetVisualizationData()
        {
            return GraphData.From(_nodes, (value, info) =>
            {
                if (value is Pointer p)
                {
                    var e = info.AddEdge(p.Target);
                    e.Color = "orange";
                    info.Color = "orange";
                    info.Label = p.Name;
                }

                foreach (var f in _closures)
                {
                    f(value, info);
                }
            });
        }
    }
}