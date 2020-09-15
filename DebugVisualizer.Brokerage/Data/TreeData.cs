using System.Collections.Generic;
using Newtonsoft.Json;

namespace DebugVisualizer.Brokerage.Data
{
    public sealed class Tree : VisualizationData
    {
        public Tree(TreeNode root)
        {
            Root = root;
        }

        public override string[] Tags => new[] {"tree"};

        [JsonProperty("root")]
        public TreeNode Root { get; set; }
    }
    
    public sealed class TreeNode : BaseTreeNode<TreeNode> {}
    
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class BaseTreeNode<TChild> where TChild : BaseTreeNode<TChild>
    {
        public BaseTreeNode()
        {
            Items = new List<StyledTreeNodeItem>();
        }
            
        public BaseTreeNode(List<StyledTreeNodeItem> items)
        {
            Items = items;
        }

        [JsonProperty("segment")] public string? Segment { get; set; }
        [JsonProperty("items")] public List<StyledTreeNodeItem> Items { get; set; }
        [JsonProperty("isMarked")] public bool IsMarked { get; set; }
        [JsonProperty("children")] public List<TChild> Children { get; set; } = new List<TChild>();
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public sealed class StyledTreeNodeItem
    {
        public static StyledTreeNodeItem Unstyled(string text)
        {
            return new StyledTreeNodeItem(text, null);
        }
        
        public static StyledTreeNodeItem Style1(string text)
        {
            return new StyledTreeNodeItem(text, "style1");
        }
        
        public static StyledTreeNodeItem Style2(string text)
        {
            return new StyledTreeNodeItem(text, "style2");
        }
        
        public static StyledTreeNodeItem Style3(string text)
        {
            return new StyledTreeNodeItem(text, "style3");
        }
        
        public StyledTreeNodeItem(string text, string? emphasis = null)
        {
            Text = text;
            Emphasis = emphasis;
        }

        [JsonProperty("text")] public string Text { get; set; }
        [JsonProperty("emphasis")] public string? Emphasis { get; set; }
        
    }
}