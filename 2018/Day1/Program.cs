using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            var seen = new HashSet<int>();
            var seed = 0;
            var running = true;
            var first = true;
            while (running)
            {
                seed = File.ReadAllLines("Input.txt").Select(int.Parse).Aggregate(seed, (acc, cur) =>
                {
                    if (seen.Contains(acc) && running)
                    {
                        Console.WriteLine(acc);
                        running = false;
                    }
                    seen.Add(acc);
                    return acc + cur;
                });
                if (first)
                {
                    Console.WriteLine(seed);
                    first = false;
                }
            }
            Console.Read();
        }
    }
}
