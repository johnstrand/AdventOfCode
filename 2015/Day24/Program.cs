using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day24
{
    internal class Program
    {
        private static long target;
        private static readonly List<HashSet<long>> combos = new List<HashSet<long>>();

        private static void Main()
        {
            var values = File.ReadAllLines("input.txt").Select(long.Parse).ToList();
            target = values.Sum() / 3;
            CreateGroups(values, target, new List<long>());
            var min = combos.Min(c => c.Count);
            var qe = combos.Where(c => c.Count == min).Select(Multiply).Min();
            Console.WriteLine($"Part 1: {qe}");

            target = values.Sum() / 4;
            combos.Clear();
            CreateGroups(values, target, new List<long>());
            min = combos.Min(c => c.Count);
            qe = combos.Where(c => c.Count == min).Select(Multiply).Min();
            Console.WriteLine($"Part 2: {qe}");
        }

        private static long Multiply(HashSet<long> values)
        {
            var res = values.Aggregate(1L, (acc, cur) => acc * cur);
            return res;
        }

        private static void CreateGroups(List<long> numbers, long sum, List<long> combo)
        {
            if (sum == 0)
            {
                combos.Add(new HashSet<long>(combo));
                return;
            }
            if (sum < 0)
            {
                return;
            }

            foreach (var number in numbers)
            {
                CreateGroups(numbers.Where(n => n > number).ToList(), sum - number, combo.Append(number).ToList());
            }
        }
    }
}