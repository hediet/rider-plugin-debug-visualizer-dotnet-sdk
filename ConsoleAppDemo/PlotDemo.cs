using System;
using System.Collections.Generic;
using DebugVisualizer.Brokerage.Data;

namespace ConsoleAppDemo
{
    static class PlotDemo
    {
        public static void Run()
        {
            // Visualize `data` here!
            var data = Plot((i, j) => Math.Sin(i) * Math.Sin(j));
        }
        
        static PlotlyData Plot(Func<double, double, double> fn)
        {
            var plot = new PlotlyPlotData()
                {Y = new List<double>(), X = new List<double>(), Z = new List<double>(), Type = PlotType.Mesh3D};
            var plotly = new PlotlyData(new[] {plot});

            for (var i = 0.0; i < 10; i += 0.1)
            {
                for (var j = 0.0; j < 10; j += 0.1)
                {
                    plot.X.Add(i);
                    plot.Y.Add(j);
                    plot.Z.Add(fn(i, j));
                }
            }

            return plotly;
        }
    }
}