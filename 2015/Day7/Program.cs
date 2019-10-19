using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day7
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var regs = new Dictionary<string, string>();
            foreach (var row in File.ReadAllLines("input.txt"))
            {
                var instr = row.Split(new[] { "->" }, StringSplitOptions.RemoveEmptyEntries).Select(str => str.Trim()).ToList();
                regs.Add(instr[1], instr[0]);
            }

            // Uncomment to enable part 2 regs["b"] = "3176";

            Console.WriteLine(Resolve(regs, "a"));

            Console.ReadLine();
        }

        private static Dictionary<string, Func<ushort, ushort, ushort>> funcs = new Dictionary<string, Func<ushort, ushort, ushort>>
        {
            { "AND", (x, y) => (ushort)(x & y) },
            { "OR", (x, y) => (ushort)(x | y) },
            { "LSHIFT", (x, y) => (ushort)(x << y) },
            { "RSHIFT", (x, y) => (ushort)(x >> y) }
        };

        private static ushort Resolve(Dictionary<string, string> regs, string name)
        {
            if (ushort.TryParse(name, out var nameVal))
            {
                return nameVal;
            }
            var expr = regs[name];
            if (ushort.TryParse(expr, out var regVal))
            {
                return regVal;
            }

            var segs = expr.Split(' ');

            if (segs.Length == 1)
            {
                var value = Resolve(regs, segs[0]);
                regs[name] = value.ToString();
                return value;
            }
            else if (segs.Length == 2)
            {
                var value = (ushort)(ushort.MaxValue - Resolve(regs, segs[1]));
                regs[name] = value.ToString();
                return value;
            }
            else
            {
                var value = funcs[segs[1]](Resolve(regs, segs[0]), Resolve(regs, segs[2]));
                regs[name] = value.ToString();
                return value;
            }
        }
    }
}