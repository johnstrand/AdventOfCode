using System;
using System.IO;
using System.Linq;

namespace Day2
{
    internal class Program
    {
        private static void Main()
        {
            var valid1 = 0;
            var valid2 = 0;
            foreach (var line in File.ReadAllLines("input.txt"))
            {
                var parts = line.Split(new[] { ' ', '-', ':' }, StringSplitOptions.RemoveEmptyEntries);
                var min = int.Parse(parts[0]);
                var max = int.Parse(parts[1]);
                var rule = parts[2][0];
                var password = parts[3];

                if (InRange(password.Count(c => c == rule), min, max))
                {
                    valid1++;
                }
                if (password[min - 1] == rule ^ password[max - 1] == rule)
                {
                    valid2++;
                }
            }
            Console.WriteLine($"Part 1: {valid1}");
            Console.WriteLine($"Part 2: {valid2}");
        }

        private static bool InRange(int value, int min, int max)
        {
            return value >= min && value <= max;
        }
    }
}