using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            var fabric = new int[1000 * 1000];
            var idIndex = new Dictionary<int, int>();
            foreach (var row in File.ReadAllLines("Input.txt"))
            {
                var m = Regex.Match(row, @"#(?<id>\d+) @ (?<start>\d+,\d+): (?<end>\d+x\d+)");
                var id = int.Parse(m.Groups["id"].Value);
                var start = Parse(m.Groups["start"].Value);
                var size = Parse(m.Groups["end"].Value);

                var key = start.X + start.Y * 1000;
                if (!idIndex.ContainsKey(key))
                    idIndex.Add(key, id);

                for (var y = start.Y; y < start.Y + size.Y; y++)
                {
                    for (var x = start.X; x < start.X + size.X; x++)
                    {
                        fabric[x + y * 1000]++;
                    }
                }
            }
            Console.WriteLine(fabric.Count(i => i >= 2));

            var intact = fabric.Select((v, i) => (v: v, i: i)).First(x => x.v == 1);

            Console.WriteLine(idIndex[intact.i]);

            Console.Read();
        }
        static Point Parse(string text)
        {
            var parts = text.Split(',', 'x').Select(int.Parse).ToList();

            return new Point(parts[0], parts[1]);
        }
    }
}
