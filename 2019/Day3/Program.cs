using System;
using System.Collections.Generic;
using System.Linq;

namespace Day3
{
    internal class Program
    {
        private static Dictionary<string, int> nodeCount = new Dictionary<string, int>();

        private static void Main(string[] args)
        {
            var wires = new[]
            {
                "R8,U5,L5,D3",
                "U7,R6,D4,L4"
            };

            foreach (var wire in wires)
            {
                Eval(wire.Split(',').ToList(), 0, 0);
            }
        }

        private static void Eval(List<string> instr, int x, int y)
        {
            var key = $"{x}_{y}";
            if (nodeCount.ContainsKey(key))
            {
                nodeCount[key]++;
            }
            else
            {
                nodeCount.Add(key, 1);
            }

            if (!instr.Any())
            {
                return;
            }

            var cur = instr.First();
            var dir = cur[0];
            var dist = int.Parse(cur.Substring(1));

            var getOffset = new Func<string, int>(axis =>
            {
                if (axis == "y" && (dir == 'L' || dir == 'R'))
                {
                    return 0;
                }

                return (dir == 'L' || dir == 'U') ? -dist : dist;
            });

            Eval(
                instr.Skip(1).ToList(),
                x + getOffset("x"),
                y + getOffset("y"));
        }
    }
}