using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Day8
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var part1 = 0;
            var part2 = 0;
            var t = Escape("\"\\x27\"");
            foreach (var line in File.ReadAllLines("input.txt"))
            {
                var initial = line.Length;
                var parsed = Parse(line);
                var escaped = Escape(line);
                Console.WriteLine($"{line} => {parsed}, {escaped}");
                part1 += initial - parsed.Length;
                part2 += escaped.Length + 2 - initial;
            }
            Console.WriteLine($"Part 1: {part1}");
            Console.WriteLine($"Part 2: {part2}");
        }

        private static string Escape(string input)
        {
            return input.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private static string Parse(string input)
        {
            var chars = new Queue<char>(input[1..^1]);
            var output = new StringBuilder();
            while (chars.Count > 0)
            {
                var next = chars.Dequeue();
                if (next == '\\')
                {
                    var sigil = chars.Dequeue();
                    if (sigil == '\\' || sigil == '"')
                    {
                        next = sigil;
                    }
                    else
                    {
                        next = (char)int.Parse(chars.Dequeue().ToString() + chars.Dequeue().ToString(), NumberStyles.HexNumber);
                    }
                }
                output.Append(next);
            }

            return output.ToString();
        }
    }
}