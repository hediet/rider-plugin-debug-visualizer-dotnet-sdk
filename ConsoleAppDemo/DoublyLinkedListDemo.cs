using System;
using System.Collections.Generic;
using System.Diagnostics;
using DebugVisualizer.Brokerage.Data;

namespace ConsoleAppDemo
{
    static class DoublyLinkedListDemo
    {
        public static void Run()
        {
            var list = new DoublyLinkedList<int>();
            // visualize `list` here!
            list.Append(1);
            list.Append(2);
            list.Append(3);
            list.Append(4);

            list.Reverse();
        }
    }


    class DoublyLinkedList<T>
    {
        class Node
        {
            public Node(T value)
            {
                Value = value;
            }

            public T Value { get; set; }
            public Node? Next { get; set; }
            public Node? Previous { get; set; }

            public void InsertAfter(Node node)
            {
                node.Next = Next;
                if (Next != null)
                {
                    Next.Previous = node.Next;
                }

                node.Previous = this;
                Next = node;
            }
        }

        private Node? Head { get; set; }
        private Node? Tail { get; set; }

        public void Append(T item)
        {
            var node = new Node(item);

            if (Head == null || Tail == null)
            {
                Debug.Assert(Head == null && Tail == null);
                Head = Tail = node;
            }
            else
            {
                Tail.InsertAfter(node);
                Tail = node;
            }
        }

        public void Reverse()
        {
            var finished = new HashSet<Node>();
            Node? last = null;
            Tail = Head;
            while (Head != null)
            {
                Head.Previous = Head.Next;
                Head.Next = last;
                finished.Add(Head);
                last = Head;
                Head = Head.Previous;
            }

            Head = last;
        }
        
        public GraphBuilder GetVisualizationData() => new GraphBuilder().WithPointer("Head", Head).WithPointer("Tail", Tail).WithClosure<Node>(
            (node, info) =>
            {
                info.Label = "" + node.Value;
                info.Id = "" + node.GetHashCode();
                info.AddEdge(node.Next, null, "Next");
                info.AddEdge(node.Previous, null, "Prev");
            });
    }
}












/*


GetVisualizationData().WithPointer("last", last).WithHighlighting(finished, "lime")

public GraphBuilder GetVisualizationData() =>
    new GraphBuilder()
        .WithPointer("Head", Head)
        .WithPointer("Tail", Tail)
        .WithClosure<Node>((node, info) =>
        {
            info.Label = "" + node.Value;
            info.Id = "" + node.GetHashCode();
            info.AddEdge(node.Next, null, "Next");
            info.AddEdge(node.Previous, null, "Prev", "lightgray");
        });

*/