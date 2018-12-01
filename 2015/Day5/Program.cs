using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day5
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var rows = File.ReadAllLines("Input.txt");

            Console.WriteLine(rows.Count(Nice1));
            Console.WriteLine(rows.Count(Nice2));
            Console.Read();
        }

        private static bool Nice1(string row)
        {
            var naughtyWords = new[] { "ab", "cd", "pq", "xy" };
            if(naughtyWords.Any(row.Contains))
            {
                return false;
            }
            var vowelChars = "aeiou";
            var vowels = 0;
            var repeat = false;
            for (var i = 0; i < row.Length; i++)
            {
                if (vowelChars.Contains(row[i]))
                {
                    vowels++;
                }
                if(i > 0 && row[i] == row[i - 1])
                {
                    repeat = true;
                }
            }

            return vowels > 2 && repeat;
        }

        private static bool Nice2(string row)
        {
            return Regex.IsMatch(row, @"(..).*?\1") && Regex.IsMatch(row, @"(.).(\1)");
        }
    }
}
