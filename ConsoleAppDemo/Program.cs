using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DebugVisualizer.Brokerage;
using DebugVisualizer.Brokerage.Data;
using DebugVisualizer.Brokerage.Brokers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleAppDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            PlotlyDemo();
            LinkedListDemo();
            ListDemo();
            AstDemo();
        }

        static void PlotlyDemo()
        {
            var numbers = new List<double>();
            var plot = new PlotlyPlotData() {Y = numbers};
            var plotly = new PlotlyData(new[] {plot});
            for (var i = 0; i < 100; i++)
            {
                numbers.Add(Math.Sin(i));
            }
        }

        static void LinkedListDemo()
        {
            var list = new LinkedList<int>();
            // visualize `list.Visualize()` here!
            list.Append(1);
            list.Append(2);
            list.Append(3);
            list.Append(4);
        }

        static void ListDemo()
        {
            var data = new ListData();

            for (var i = 0; i < 100000; i++)
            {
                if (i % 2 == 0)
                {
                    data.Items.Add(new Dictionary<string, object>() {["name"] = "John Doe", ["y"] = Math.Sin(i)});
                }
                else
                {
                    data.Items.Add(new Dictionary<string, object>() {["name"] = "Jane doe", ["y"] = Math.Sin(i)});
                }
            }
        }

        static void AstDemo()
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

            var src = string.Join(",", Enumerable.Repeat(programText, 1));

            VisualizationBrokerService.MainVisualizationBroker.DataExtractors.Add(new RoslynVisualizationBroker());
            SyntaxTree tree = CSharpSyntaxTree.ParseText(src);

            var root = tree.GetRoot();
            foreach (var n in root.DescendantNodes())
            {
                //var x = DataExtractor.Extract(n).Parse().ExtractedData.Data.ToString();
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

    class LinkedList<T>
    {
        class Node
        {
            public Node(T value)
            {
                Value = value;
            }

            public T Value { get; set; }
            public Node? Next { get; set; }
        }

        private Node? Head { get; set; }

        public void Append(T item)
        {
            if (this.Head == null)
            {
                this.Head = new Node(item);
            }
            else
            {
                var cur = this.Head;
                while (cur.Next != null)
                {
                    cur = cur.Next;
                }

                cur.Next = new Node(item);
            }
        }

        string GetVisualizationData()
        {
            var list = new Node(default(T)!) {Next = this.Head};
            return GraphData.From(new[] {list}, (item, info) =>
            {
                info.Label = item == list ? "List" : $"{item.Value}";
                if (item == list)
                {
                    info.Color = "orange";
                }

                if (item.Next != null)
                {
                    info.AddEdge(item.Next!, label: item == list ? "head" : "next");
                }

                return info;
            }).ToString();
        }
    }
}