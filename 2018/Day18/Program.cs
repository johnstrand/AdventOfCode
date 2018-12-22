using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Day18
{
    class Program
    {
        static void Main(string[] args)
        {
            var state = string.Join("", File.ReadAllLines("Input.txt")).ToCharArray();
            var limit = 100000000;
            var next = 0;
            for (var min = 0; min < limit; min++)
            {
                Display(state, 50);
                //Console.WriteLine();
                //Console.WriteLine($"Time slice: {min}");
                //Thread.Sleep(1000);
                if (min > next)
                {
                    next += 1000;
                    Console.Write($"{min.ToString("N0")} / {limit.ToString("N0")}\r");
                }
                
                state = Mutate(state, 50, 50);
            }
            var trees = state.Count(c => c == '|');
            var yards = state.Count(c => c == '#');
            Console.Write($"Resource count: {trees} * {yards} = {trees * yards}. ");

            Console.Read();
        }

        static void Display(char[] state, int w)
        {
            //Console.SetCursorPosition(0, 0);
            //var b = string.Join("", state.Select((c, i) => c.ToString() + (i > 0 && i % w == 0 ? Environment.NewLine : "")));
            //Console.Write(b);
        }

        static char[] Mutate(char[] state, int w, int h)
        {
            return state.Select((grid, index) =>
                Map(grid, SurroundingIndices(index, w, h).Select(i => state[i]).ToArray())).ToArray();
        }

        static char Map(char value, char[] surrounding)
        {
            var s = (from x in surrounding group x by x into g select new { g.Key, Count = g.Count() }).ToDictionary(x => x.Key, x => x.Count);
            if (value == '.')
            {
                return s.ContainsKey('|') && s['|'] > 2 ? '|' : '.';
            }

            if (value == '|')
            {
                return s.ContainsKey('#') && s['#'] > 2 ? '#' : '|';
            }

            if (value == '#')
            {
                return s.ContainsKey('#') && s.ContainsKey('|') ? '#' : '.';
            }
            return value;
        }

        static IEnumerable<int> SurroundingIndices(int index, int w, int h)
        {
            var x = index % w;
            var y = index / w;

            for (var ty = Math.Max(0, y - 1); ty <= Math.Min(y + 1, h - 1); ty++)
            {
                for (var tx = Math.Max(0, x - 1); tx <= Math.Min(x + 1, w - 1); tx++)
                {
                    if (tx == x && ty == y)
                    {
                        continue;
                    }

                    yield return ty * w + tx;
                }
            }
        }
    }
}
