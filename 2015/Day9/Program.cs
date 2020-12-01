using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Day9
{
    internal class Program
    {
        private static readonly Dictionary<string, int> routes = new Dictionary<string, int>();
        private static int max = int.MinValue;
        private static int min = int.MaxValue;

        private static void Main()
        {
            var places = new HashSet<string>();
            foreach (var line in File.ReadAllLines("input.txt"))
            {
                var parts = line.Split(' ');
                places.Add(parts[0]);
                places.Add(parts[2]);
                routes.Add($"{parts[0]}_{parts[2]}", int.Parse(parts[4]));
                routes.Add($"{parts[2]}_{parts[0]}", int.Parse(parts[4]));
            }
            Parallel.ForEach(places, item =>
            {
                Console.WriteLine($"Starting from {item}");
                Travel(item, places.Where(p => p != item).ToList(), 0);
            });

            Console.WriteLine($"Min {min}");
            Console.WriteLine($"Max {max}");
        }

        private static void Travel(string from, IEnumerable<string> remaining, int distance)
        {
            if (!remaining.Any())
            {
                lock (routes)
                {
                    max = Math.Max(distance, max);
                    min = Math.Min(distance, min);
                }
            }
            foreach (var to in remaining)
            {
                Travel(to, remaining.Where(r => r != to).ToList(), distance + routes[$"{from}_{to}"]);
            }
        }
    }
}