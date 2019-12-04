using System;
using System.Collections.Generic;
using System.Linq;

namespace Day4
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var min = 387638;
            var max = 919123;
            var valid = 0;
            for (var current = min; current <= max; current++)
            {
                if (Valid(current))
                {
                    valid++;
                }
            }

            Console.WriteLine(valid);
        }

        private static bool Valid(int no)
        {
            var segs = no.ToString().Select(c => c - '0').ToList();
            var pair = false;
            for (var i = 0; i < segs.Count - 1; i++)
            {
                if (segs[i] > segs[i + 1])
                {
                    return false;
                }
                pair = pair || CheckPair(segs, i);
            }

            return pair;
        }

        private static bool CheckPair(List<int> list, int index)
        {
            return list[index] == list[index + 1] &&
                // Part 2
                (index == 0 || list[index - 1] != list[index]) &&
                (index >= list.Count - 2 || list[index + 2] != list[index]);
        }
    }
}