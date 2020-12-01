using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day1
{
    internal class Program
    {
        private static void Main()
        {
            var values = new List<int>();
            foreach (var line in File.ReadAllLines("input.txt"))
            {
                values.Add(int.Parse(line));
            }

            var skip = 0;

            var p2 = false;

            foreach (var v1 in values)
            {
                skip++;
                foreach (var v2 in values.Skip(skip))
                {
                    if (v1 + v2 == 2020)
                    {
                        Console.WriteLine($"Part 1: {v1 * v2}");
                    }
                    if (p2)
                    {
                        continue;
                    }
                    foreach (var v3 in values.Skip(skip + 1))
                    {
                        if (v1 + v2 + v3 == 2020)
                        {
                            Console.WriteLine($"Part 2: {v1 * v2 * v3}");
                            p2 = true;
                        }
                    }
                }
            }
        }
    }
}