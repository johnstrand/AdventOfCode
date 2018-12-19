using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Day16
{
    class Program
    {
        static void Main(string[] args)
        {
            //Machine.Discover(new[] { 3, 2, 1, 1 }, new[] { 3, 2, 2, 1 }, new Operation { Code = 9, A = 2, B = 1, C = 2 }).ToList();
            using (var reader = new StreamReader("Input.txt"))
            {
                while(!reader.EndOfStream)
                {
                    var initialState = Machine.ParseState(reader.ReadLine());
                    var op = Operation.Parse(reader.ReadLine());
                    var targetState = Machine.ParseState(reader.ReadLine());
                    var candidates = Machine.Discover(initialState, targetState, op).ToList();
                    Console.WriteLine($"Found {candidates.Count} candidate(s) for {op}::({string.Join(", ", initialState)}) -> {string.Join(", ", targetState)}");
                }
                Console.Read();
            }
        }
    }

    class Operation
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

    static class Machine
    {
        static readonly Dictionary<string, Func<int, int, int, int[], int[]>> opcodes = new Dictionary<string, Func<int, int, int, int[], int[]>>();

        public static int[] ParseState(string input)
        {
            return Regex.Match(input, @"\[(.+)\]").Groups[1].Value.Split(',').Select(int.Parse).ToArray();
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
            
            foreach(var oc in opcodes)
            {
                var computed = oc.Value(op.A, op.B, op.C, initialState.ToArray());
                if(Eq(computed, targetState))
                {
                    yield return oc.Key;
                }
            }
        }

        private static bool Eq(int[] a, int[] b)
        {
            for(var index = 0; index < a.Length; index++)
            {
                if(a[index] != b[index])
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
