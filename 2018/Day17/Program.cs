using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Day17
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var points = GetPoints().ToList();
            var offset = points.Select(p => p.X).Min();
            foreach(var point in points)
            {
                Console.SetCursorPosition(point.X - offset, point.Y);
                Console.Write('#');
            }
            Console.SetCursorPosition(500 - offset, 0);
            Console.Write("+");
            Console.Read();
        }

        private static IEnumerable<Point> GetPoints()
        {
            foreach (var row in File.ReadAllLines("Input.txt"))
            {
                var parts = row.Split(',').ToDictionary(
                    x => x.Split('=').First().Trim(),
                    x => x.Split('=').Last().Trim().Split("..").Select(int.Parse).ToList());

                for (var y = parts["y"].Min(); y <= parts["y"].Max(); y++)
                {
                    for (var x = parts["x"].Min(); x <= parts["x"].Max(); x++)
                    {
                        yield return new Point(x, y);
                    }
                }
            }
        }
    }
}
