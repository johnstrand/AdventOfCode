using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var buckets = new Dictionary<int, HashSet<int>>();
            foreach (var line in File.ReadAllLines("input.txt"))
            {
                var bucket = line.Length;
                if (!buckets.ContainsKey(bucket))
                {
                    buckets.Add(bucket, new HashSet<int>());
                }
                buckets[bucket].Add(int.Parse(line));
            }

            foreach (var v1 in buckets[3])
            {
                foreach (var v2 in buckets[4])
                {
                    if (v1 + v2 == 2020)
                    {
                        Console.WriteLine(v1 * v2);
                    }
                }
            }

            var values = buckets[3].Union(buckets[4]).ToList();

            var triples = (from v1 in values from v2 in values from v3 in values select new { sum = v1 + v2 + v3, product = v1 * v2 * v3 }).ToList();
            var triple = triples.First(t => t.sum == 2020);
            Console.WriteLine(triple.product);
        }
    }
}