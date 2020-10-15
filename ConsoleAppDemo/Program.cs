using DebugVisualizer.Brokerage.Data;

namespace ConsoleAppDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = typeof(VisualizationData);
            //DoublyLinkedListDemo.Run();
            //PlotDemo.Run();
            ListDemo.Run();
            AstDemo.Run();
            //ILDemo.Run();
            //TypeHierarchyDemo.Run();
        }
    }
}