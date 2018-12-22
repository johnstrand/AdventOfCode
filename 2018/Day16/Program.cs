using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading;

namespace Day16
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Log.EnableVerbose = true;
            var opcodeCandidates = new Dictionary<int, HashSet<string>>();
            using (var reader = new StreamReader("Input.txt"))
            {
                var count = 0;
                while (!reader.EndOfStream)
                {
                    var header = reader.ReadLine();
                    if (header == "")
                    {
                        break;
                    }
                    var initialState = Machine.ParseState(header);
                    var op = Operation.Parse(reader.ReadLine());
                    var targetState = Machine.ParseState(reader.ReadLine());
                    var candidates = Machine.Discover(initialState, targetState, op).ToList();
                    if (!opcodeCandidates.ContainsKey(op.Code))
                    {
                        opcodeCandidates.Add(op.Code, new HashSet<string>(candidates));
                    }
                    else
                    {
                        opcodeCandidates[op.Code] = new HashSet<string>(candidates.Where(opcodeCandidates[op.Code].Contains));
                    }
                    reader.ReadLine(); // Skip blank
                    if (candidates.Count > 2)
                    {
                        count++;
                    }
                }
                Log.Write($"Found {count} samples matching 3 or more op codes");
                while (opcodeCandidates.Any(c => c.Value.Count > 1))
                {
                    var singles = new HashSet<string>(opcodeCandidates.Where(c => c.Value.Count == 1).SelectMany(c => c.Value));
                    foreach (var c in opcodeCandidates)
                    {
                        if (c.Value.Count == 1)
                        {
                            continue;
                        }
                        c.Value.RemoveWhere(singles.Contains);
                    }
                }
                var opcodes = opcodeCandidates.ToDictionary(v => v.Key, v => v.Value.First());
                var regs = new int[4];
                while (!reader.EndOfStream)
                {
                    var row = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(row))
                    {
                        continue;
                    }
                    Console.Write($"[{string.Join(", ", regs.Select(r => r.ToString("000")))}]\r");
                    var op = Operation.Parse(row);
                    regs = Machine.Execute(opcodes[op.Code], regs, op);
                    Thread.Sleep(10);
                }
                Console.Write($"[{string.Join(", ", regs.Select(r => r.ToString("000")))}]\r");
                Console.WriteLine();
                Console.Read();
            }
        }
    }

    internal class Log
    {
        public static bool EnableVerbose;
        public static void Write(string text)
        {
            Console.WriteLine(text);
        }
        public static void Verbose(string text)
        {
            if (!EnableVerbose)
            {
                return;
            }
            Write(text);
        }
    }

    internal class Operation
    {
        public int Code { get; set; }
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }

        public static Operation Parse(string input)
        {
            var parts = input.Split(' ').Select(int.Parse).ToArray();
            return new Operation { Code = parts[0], A = parts[1], B = parts[2], C = parts[3] };
        }

        public override string ToString()
        {
            return $"{Code}({A}, {B}) -> {C}";
        }
    }

    internal static class Machine
    {
        private static readonly Dictionary<string, Func<int, int, int, int[], int[]>> opcodes = new Dictionary<string, Func<int, int, int, int[], int[]>>();

        public static int[] ParseState(string input)
        {
            return Regex.Match(input, @"\[(.+)\]").Groups[1].Value.Split(',').Select(int.Parse).ToArray();
        }
        public static int[] Execute(string code, int[] state, Operation op)
        {
            return opcodes[code](op.A, op.B, op.C, state);
        }
        public static IEnumerable<string> Discover(int[] initialState, int[] targetState, Operation op)
        {
            if (!opcodes.Any())
            {
                var codes = typeof(Machine).GetMethods().Where(m => m.ReturnType == typeof(int[])).ToList();

                foreach (var code in codes)
                {
                    var p = new[]
                    {
                        Expression.Parameter(typeof(int), "a"),
                        Expression.Parameter(typeof(int), "b"),
                        Expression.Parameter(typeof(int), "c"),
                        Expression.Parameter(typeof(int[]), "regs")
                    };
                    try
                    {
                        var f = Expression.Lambda<Func<int, int, int, int[], int[]>>(Expression.Call(code, p), p).Compile();

                        opcodes.Add(code.Name, f);
                    }
                    catch
                    {

                    }
                }
            }

            foreach (var oc in opcodes)
            {
                var computed = oc.Value(op.A, op.B, op.C, initialState.ToArray());
                if (Eq(computed, targetState))
                {
                    yield return oc.Key;
                }
            }
        }

        private static bool Eq(int[] a, int[] b)
        {
            for (var index = 0; index < a.Length; index++)
            {
                if (a[index] != b[index])
                {
                    return false;
                }
            }

            return true;
        }

        public static int[] Addr(int a, int b, int c, int[] regs)
        {
            regs[c] = regs[a] + regs[b];
            return regs;
        }
        public static int[] Addi(int a, int b, int c, int[] regs)
        {
            regs[c] = regs[a] + b;
            return regs;
        }
        public static int[] Mulr(int a, int b, int c, int[] regs)
        {
            regs[c] = regs[a] * regs[b];
            return regs;
        }
        public static int[] Muli(int a, int b, int c, int[] regs)
        {
            regs[c] = regs[a] * b;
            return regs;
        }
        public static int[] Banr(int a, int b, int c, int[] regs)
        {
            regs[c] = regs[a] & regs[b];
            return regs;
        }
        public static int[] Bani(int a, int b, int c, int[] regs)
        {
            regs[c] = regs[a] & b;
            return regs;
        }
        public static int[] Borr(int a, int b, int c, int[] regs)
        {
            regs[c] = regs[a] | regs[b];
            return regs;
        }
        public static int[] Bori(int a, int b, int c, int[] regs)
        {
            regs[c] = regs[a] | b;
            return regs;
        }
        public static int[] Setr(int a, int b, int c, int[] regs)
        {
            regs[c] = regs[a];
            return regs;
        }
        public static int[] Seti(int a, int b, int c, int[] regs)
        {
            regs[c] = a;
            return regs;
        }
        public static int[] Gtir(int a, int b, int c, int[] regs)
        {
            regs[c] = a > regs[b] ? 1 : 0;
            return regs;
        }
        public static int[] Gtri(int a, int b, int c, int[] regs)
        {
            regs[c] = regs[a] > b ? 1 : 0;
            return regs;
        }
        public static int[] Gtrr(int a, int b, int c, int[] regs)
        {
            regs[c] = regs[a] > regs[b] ? 1 : 0;
            return regs;
        }
        public static int[] Eqir(int a, int b, int c, int[] regs)
        {
            regs[c] = a == regs[b] ? 1 : 0;
            return regs;
        }
        public static int[] Eqri(int a, int b, int c, int[] regs)
        {
            regs[c] = regs[a] == b ? 1 : 0;
            return regs;
        }
        public static int[] Eqrr(int a, int b, int c, int[] regs)
        {
            regs[c] = regs[a] == regs[b] ? 1 : 0;
            return regs;
        }
    }
}
