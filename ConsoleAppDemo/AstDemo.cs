using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using DebugVisualizer.Brokerage;
using DebugVisualizer.Brokerage.Data;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleAppDemo
{
    static class AstDemo
    {
        public static void Run()
        {
            string programText = @"using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello, World!"");
        }
    }
}";

            SyntaxTree tree = CSharpSyntaxTree.ParseText(programText);
            
            VisualizationBrokerService.MainVisualizationBroker.DataExtractors.Add(new RoslynVisualizationBroker());
            
            var root = tree.GetRoot();
            foreach (var n in root.DescendantNodes())
            {
                Console.WriteLine(n);
            }
        }
    }
    
    internal class RoslynVisualizationBroker : IVisualizationBroker
    {
        public void Broker(object? value, IVisualizationBrokerContext context)
        {
            if (value is SyntaxTree tree)
            {
                value = tree.GetRoot();
            }

            if (value is SyntaxNode node)
            {
                var root = node;
                while (root.Parent != null)
                {
                    root = root.Parent;
                }

                context.Add(
                    () => new AstData(GetNode(root, node, false), root.GetText().ToString()) {FileName = "code.cs"},
                    new VisualizationBrokerInfo("roslyn.SyntaxTree", "Roslyn Syntax Tree", 1000)
                );
                context.Add(
                    () => new AstData(GetNode(root, node, true), root.GetText().ToString()) {FileName = "code.cs"},
                    new VisualizationBrokerInfo("roslyn.SyntaxTreeWithTokens", "Roslyn Syntax Tree With Tokens", 900)
                );
            }
        }

        private static AstTreeNode GetNode(SyntaxNodeOrToken node, SyntaxNode? marked, bool includeTokens)
        {
            static bool AreEqual(object? obj1, SyntaxNodeOrToken obj2)
            {
                return obj2.IsNode && obj1 == obj2.AsNode() || obj2.IsToken && obj2.AsToken().Equals(obj1);
            }

            string? propName = null;
            if (node.Parent != null)
            {
                var properties = node.Parent.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                propName = properties.Select(propertyInfo =>
                {
                    var propertyVal = propertyInfo.GetValue(node.Parent);
                    if (AreEqual(propertyVal, node)) return propertyInfo.Name;
                    if (propertyVal is IEnumerable e)
                    {
                        var idx = 0;
                        foreach (var c in e)
                        {
                            if (AreEqual(c, node)) return $"{propertyInfo.Name}[{idx}]";
                            idx++;
                        }
                    }

                    return null;
                }).FirstOrDefault(n => n != null);
            }

            var result = new AstTreeNode(new AstSpan {Start = node.Span.Start, Length = node.Span.Length})
            {
                Children =
                    (includeTokens ? node.ChildNodesAndTokens() : node.ChildNodesAndTokens().Where(t => t.IsNode))
                    .Select(n => GetNode(n, marked, includeTokens)).ToList(),
                IsMarked = node == marked,
                Segment = "." + propName
            };

            var kindName = node.Kind().ToString();
            if (propName != null)
            {
                result.Items.Add(StyledTreeNodeItem.Style1(propName));
                result.Items.Add(StyledTreeNodeItem.Unstyled($": {kindName}"));
            }
            else
            {
                result.Items.Add(StyledTreeNodeItem.Unstyled(kindName));
            }

            if (node.AsNode() is IdentifierNameSyntax s)
            {
                result.Items.Add(StyledTreeNodeItem.Style2(s.Identifier.ValueText));
            }

            if (node.AsNode() is PredefinedTypeSyntax s2)
            {
                result.Items.Add(StyledTreeNodeItem.Style2(s2.Keyword.Text));
            }
            
            return result;
        }
    }
}