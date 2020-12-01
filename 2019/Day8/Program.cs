using System;
using System.IO;
using System.Linq;

namespace Day8
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var w = 25;
            var h = 6;
            var chunkSize = w * h;
            var layers = string.Join("",
                File.ReadAllText("input.txt")
                    .Select((c, i) =>
                        (i > 1 && (i % chunkSize) == 0 ? "\n" : "") + c.ToString()).ToList()).Split('\n');

            var zeroCount = int.MaxValue;
            foreach (var layer in layers)
            {
                var counts = layer.GroupBy(c => c).ToDictionary(kv => kv.Key - '0', kv => kv.Count());
                if (counts[0] < zeroCount)
                {
                    zeroCount = counts[0];
                    Console.WriteLine(counts[1] * counts[2]);
                }
            }

            for (var y = 0; y < h; y++)
            {
                for (var x = 0; x < w; x++)
                {
                    var pixel = layers.SkipWhile(layer => layer[y * w + x] == '2').First()[y * w + x];
                    if (pixel == '1')
                    {
                        Console.Write("*");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}