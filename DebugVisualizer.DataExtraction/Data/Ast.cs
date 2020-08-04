using Newtonsoft.Json;

namespace DebugVisualizer.DataExtraction.Data
{
    public sealed class Ast : VisualizationData
    {
        public Ast(TreeNode<AstData> root, string text)
        {
            Root = root;
            Text = text;
        }

        public override string[] Tags => new[] {"tree", "text", "ast"};

        [JsonProperty("root")]
        public TreeNode<AstData> Root { get; set; }
        
        [JsonProperty("text")]
        public string Text { get; set; }
    }
    
    public sealed class AstData
    {
        [JsonProperty("position")] public int Position { get; set; }
        [JsonProperty("length")] public int Length { get; set; }
    }
}