using System;
using System.Linq;

namespace ConsoleAppDemo
{
    class Contact
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    static class ListDemo
    {
        public static void Run()
        {
            var list = Enumerable.Range(0, 10000).Select(i => new Contact()
            {
                Name = i % 2 == 0 ? "John Doe" : "Jane Doe",
                X = (int) (Math.Sin(i / 10.0) * 100 * i),
                Y = (int) (Math.Cos(i / 10.0) * 100 * i)
            }).ToList();
        }
    }
}