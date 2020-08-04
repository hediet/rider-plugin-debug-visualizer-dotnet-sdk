using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DebugVisualizer.DataExtraction;
using DebugVisualizer.DataExtraction.Data;
using DebugVisualizer.DataExtraction.Extractors;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleAppDemo
{
    
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = new List<double>();
            var plot = new PlotlyPlotData() {Y = numbers};
            var plotly = new PlotlyData(new[] {plot});
            for (var i = 0; i < 100; i++)
            {
                numbers.Add(Math.Sin(i));
            }
            
            var list = new LinkedList<int>();
            // visualize `list.Visualize()` here!
            list.Append(1);
            list.Append(2);
            list.Append(3);
            list.Append(4);

            
            const string programText = @"using System;
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

            DataExtractor.MainDataExtractor.DataExtractors.Add(new RoslynDataExtractor());
            SyntaxTree tree = CSharpSyntaxTree.ParseText(programText);
        }
    }

    class RoslynDataExtractor : GenericDataExtractor<SyntaxTree>
    {
        public override void GetExtractions(SyntaxTree value, IDataExtractorContext context)
        {
            context.AddExtraction(
                () => new Ast(GetNode(value.GetRoot()), value.GetText().ToString()),
                new DataExtractorInfo("roslyn.SyntaxTree", "Roslyn Syntax Tree", 1000)
            );
        }

        private TreeNode<AstData> GetNode(SyntaxNodeOrToken node)
        {
            string? id = null;
            if (node.Parent != null)
            {
                var type = node.Parent.GetType();
                foreach (var m in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var val = m.GetValue(node.Parent);
                    if (val == node.AsNode() || node.AsToken().Equals(val))
                    {
                        id = m.Name;
                        break;
                    }
                }
            }

            var result = new TreeNode<AstData>(node.Kind().ToString())
            {
                Data = new AstData() {Position = node.Span.Start, Length = node.Span.Length},
                Children = node.ChildNodesAndTokens().Select(GetNode).ToList(),
                Id = id
            };


            if (node.AsNode() is IdentifierNameSyntax s)
            {
                result.Value = s.Identifier.ValueText;
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