﻿using System;
using System.Collections.Generic;
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
            var idIndex = new Dictionary<int, List<int>>();
            foreach (var row in File.ReadAllLines("Input.txt"))
            {
                var m = Regex.Match(row, @"#(?<id>\d+) @ (?<start>\d+,\d+): (?<end>\d+x\d+)");
                var id = int.Parse(m.Groups["id"].Value);
                var start = Parse(m.Groups["start"].Value);
                var size = Parse(m.Groups["end"].Value);

                idIndex.Add(id, new List<int>());

                for (var y = start.Y; y < start.Y + size.Y; y++)
                {
                    for (var x = start.X; x < start.X + size.X; x++)
                    {
                        fabric[x + y * 1000]++;
                        idIndex[id].Add(x + y * 1000);
                    }
                }
            }
            Console.WriteLine(fabric.Count(i => i >= 2));

            var intact = idIndex.FirstOrDefault(ix => ix.Value.All(i => fabric[i] == 1));

            Console.WriteLine(intact.Key);

            Console.Read();
        }

        static Point Parse(string text)
        {
            var parts = text.Split(',', 'x').Select(int.Parse).ToList();

            return new Point(parts[0], parts[1]);
        }
    }

    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
