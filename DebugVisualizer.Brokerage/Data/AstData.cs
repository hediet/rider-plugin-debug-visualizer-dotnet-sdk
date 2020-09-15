using Newtonsoft.Json;

namespace DebugVisualizer.Brokerage.Data
{
    public sealed class AstData : VisualizationData
    {
        public AstData(AstTreeNode root, string text)
        {
            Root = root;
            Text = text;
        }

        public override string[] Tags => new[] {"tree", "text", "ast"};

        [JsonProperty("root")]
        public AstTreeNode Root { get; set; }
        
        [JsonProperty("text")]
        public string Text { get; set; }
        
        [JsonProperty("fileName")]
        public string FileName { get; set; }
    }
    
    public sealed class AstTreeNode : BaseTreeNode<AstTreeNode>
    {
        public AstTreeNode(AstSpan span)
        {
            Span = span;
        }
        
        [JsonProperty("span")]
        private AstSpan Span { get; set; }
    }

    public sealed class AstSpan
    {
        [JsonProperty("start")] public int Start { get; set; }
        [JsonProperty("length")] public int Length { get; set; }
    }
}