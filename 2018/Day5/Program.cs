using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText(@"Input.txt").Trim();
            Part1(input);
            Part2(input);
            Console.ReadLine();
        }

        private static void Part1(string input)
        {
            input = Reduce(input);
            Console.WriteLine($"Entire chain reduced {input.Length}");
        }

        private static void Part2(string input)
        {
            var variants = input.ToLower().Distinct();
            var min = int.MaxValue;
            foreach(var variant in variants)
            {
                var filtered = new string(input.Where(c => char.ToLower(c) != variant).ToArray());
                min = Math.Min(min, Reduce(filtered).Length);
            }

            Console.WriteLine($"Shortest reduced chain: {min}");
        }

        private static string Reduce(string input)
        {
            var filtered = false;
            do
            {
                var rest = new StringBuilder();
                filtered = false;
                for (var index = 0; index < input.Length; index++)
                {
                    if (index < input.Length - 1 && Math.Abs(input[index] - input[index + 1]) == 32)
                    {
                        index++;
                        filtered = true;
                        continue;
                    }
                    rest.Append(input[index]);
                }
                input = rest.ToString();
            }
            while (filtered);
            return input;
        }
    }
}
