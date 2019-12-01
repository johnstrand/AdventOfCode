using System;
using System.IO;
using System.Linq;

namespace Day1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var part2 = true;
            var sum = 0;
            foreach (var row in File.ReadAllLines("input.txt").Select(int.Parse))
            {
                var req = CalcFuel(row, part2);
                Console.WriteLine($"{row} = {req}");
                sum += req;
            }
            Console.WriteLine(sum);
        }

        private static int CalcFuel(int mass, bool rec)
        {
            if (mass == 0)
            {
                return 0;
            }

            var fuel = Math.Max(0, mass / 3 - 2);
            return fuel + (rec ? CalcFuel(fuel, rec) : 0);
        }
    }
}