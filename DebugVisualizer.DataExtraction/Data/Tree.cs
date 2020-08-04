using System.Collections.Generic;
using Newtonsoft.Json;

namespace DebugVisualizer.DataExtraction.Data
{
    public sealed class Tree<TData> : VisualizationData
    {
        public Tree(TreeNode<TData> root)
        {
            Root = root;
        }

        public override string[] Tags => new[] {"tree"};

        [JsonProperty("root")]
        public TreeNode<TData> Root { get; set; }
    }
    
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public sealed class TreeNode<TData>
    {
        public TreeNode(string name)
        {
            Name = name;
        }

        [JsonProperty("id")] public string? Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("value")] public string? Value { get; set; }
        [JsonProperty("emphasizedValue")] public string? EmphasizedValue { get; set; }
        [JsonProperty("isMarked")] public bool IsMarked { get; set; }
        [JsonProperty("data")] public TData Data { get; set; } = default(TData)!;
        [JsonProperty("children")] public List<TreeNode<TData>> Children { get; set; } = new List<TreeNode<TData>>();
    }
}