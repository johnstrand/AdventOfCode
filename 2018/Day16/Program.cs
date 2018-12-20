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
            Log.EnableVerbose = true;
            var opcodes = new Dictionary<int, HashSet<string>>();
            using (var reader = new StreamReader("Input.txt"))
            {
                var count = 0;
                while(!reader.EndOfStream)
                {
                    var header = reader.ReadLine();
                    if(header == "")
                    {
                        break;
                    }
                    var initialState = Machine.ParseState(header);
                    var op = Operation.Parse(reader.ReadLine());
                    var targetState = Machine.ParseState(reader.ReadLine());
                    var candidates = Machine.Discover(initialState, targetState, op).ToList();
                    if(!opcodes.ContainsKey(op.Code))
                    {
                        opcodes.Add(op.Code, new HashSet<string>(candidates));
                    }
                    else
                    {
                        Log.Verbose($"Existing candidates for op code {op.Code}:");
                        Log.Verbose(string.Join(", ", opcodes[op.Code]));
                        Log.Verbose("New candiate set:");
                        Log.Verbose(string.Join(", ", candidates));
                        opcodes[op.Code] = new HashSet<string>(candidates.Where(opcodes[op.Code].Contains));
                    }
                    reader.ReadLine(); // Skip blank
                    Log.Write($"Found {candidates.Count} candidate(s) for {op}::({string.Join(", ", initialState)}) -> {string.Join(", ", targetState)}");
                    if(candidates.Count > 2)
                    {
                        count++;
                    }
                    Log.Write("");
                }
                Log.Write($"Found {count} samples matching 3 or more op codes");
                Console.Read();
            }
        }
    }
    class Log
    {
        public static bool EnableVerbose;
        public static void Write(string text)
        {
            Console.WriteLine(text);
        }
        public static void Verbose(string text)
        {
            if(!EnableVerbose)
            {
                return;
            }
            Write(text);
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
                Log.Verbose($"{oc.Key}(A: {op.A}, B: {op.B}, C: {op.C})");
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
            Log.Verbose($"Addr: regs[{c}] = regs[{a}] + regs[{b}] = {regs[a] + regs[b]}");
            regs[c] = regs[a] + regs[b];
            return regs;
        }
        public static int[] Addi(int a, int b, int c, int[] regs)
        {
            Log.Verbose($"Addi: regs[{c}] = regs[{a}] + {b} = {regs[a] + b}");
            regs[c] = regs[a] + b;
            return regs;
        }
        public static int[] Mulr(int a, int b, int c, int[] regs)
        {
            Log.Verbose($"Mulr: regs[{c}] = regs[{a}] * regs[{b}] = {regs[a] * regs[b]}");
            regs[c] = regs[a] * regs[b];
            return regs;
        }
        public static int[] Muli(int a, int b, int c, int[] regs)
        {
            Log.Verbose($"Muli: regs[{c}] = regs[{a}] * b = {regs[a] * b}");
            regs[c] = regs[a] * b;
            return regs;
        }
        public static int[] Banr(int a, int b, int c, int[] regs)
        {
            Log.Verbose($"Banr: regs[{c}] = regs[{a}] & regs[{b}] = {regs[a] & regs[b]}");
            regs[c] = regs[a] & regs[b];
            return regs;
        }
        public static int[] Bani(int a, int b, int c, int[] regs)
        {
            Log.Verbose($"Bani: regs[{c}] = regs[{a}] & b = {regs[a] & b}");
            regs[c] = regs[a] & b;
            return regs;
        }
        public static int[] Borr(int a, int b, int c, int[] regs)
        {
            Log.Verbose($"Borr: regs[{c}] = regs[{a}] | regs[{b}] = {regs[a] | regs[b]}");
            regs[c] = regs[a] | regs[b];
            return regs;
        }
        public static int[] Bori(int a, int b, int c, int[] regs)
        {
            Log.Verbose($"Bori: regs[{c}] = regs[{a}] | b = {regs[a] | b}");
            regs[c] = regs[a] | b;
            return regs;
        }
        public static int[] Setr(int a, int b, int c, int[] regs)
        {
            Log.Verbose($"Setr: regs[{c}] = regs[{a}] = {regs[a]}");
            regs[c] = regs[a];
            return regs;
        }
        public static int[] Seti(int a, int b, int c, int[] regs)
        {
            Log.Verbose($"Seti: regs[{c}] = a = {a}");
            regs[c] = a;
            return regs;
        }
        public static int[] Gtir(int a, int b, int c, int[] regs)
        {
            Log.Verbose($"Gtir: regs[{c}] = a > regs[{b}] = {(a > regs[b] ? 1 : 0)}");
            regs[c] = a > regs[b] ? 1 : 0;
            return regs;
        }
        public static int[] Gtri(int a, int b, int c, int[] regs)
        {
            Log.Verbose($"Gtri: regs[{c}] = regs[{a}] = {(regs[a] > b ? 1 : 0)}");
            regs[c] = regs[a] > b ? 1 : 0;
            return regs;
        }
        public static int[] Gtrr(int a, int b, int c, int[] regs)
        {
            Log.Verbose($"Gtrr: regs[{c}] = regs[{a}] > regs[{b}] = {(regs[a] > regs[b] ? 1 : 0)}");
            regs[c] = regs[a] > regs[b] ? 1 : 0;
            return regs;
        }
        public static int[] Eqir(int a, int b, int c, int[] regs)
        {
            Log.Verbose($"Eqir: regs[{c}] = a == regs[{b}] = {(a == regs[b] ? 1 : 0)}");
            regs[c] = a == regs[b] ? 1 : 0;
            return regs;
        }
        public static int[] Eqri(int a, int b, int c, int[] regs)
        {
            Log.Verbose($"Eqri: regs[{c}] = regs[{a}] == b = {(regs[a] == b ? 1 : 0)}");
            regs[c] = regs[a] == b ? 1 : 0;
            return regs;
        }
        public static int[] Eqrr(int a, int b, int c, int[] regs)
        {
            Log.Verbose($"Eqrr: regs[{c}] = regs[{a}] == regs[{b}] = {(regs[a] == regs[b] ? 1 : 0)}");
            regs[c] = regs[a] == regs[b] ? 1 : 0;
            return regs;
        }
    }
}
