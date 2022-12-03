using System.Diagnostics.CodeAnalysis;
using System.Reflection;

List<int> prg = null;

int ptr = 0;

prg = File.ReadAllText("input.txt").Split(",").Select(int.Parse).ToList();
var cache = new Dictionary<Opcodes, (MethodInfo method, OpcodeAttribute meta)>();

foreach (var method in typeof(Program).GetMethods(BindingFlags.Static | BindingFlags.NonPublic))
{
    OpcodeAttribute attr;
    if ((attr = method.GetCustomAttribute<OpcodeAttribute>()) == null)
    {
        continue;
    }

    cache.Add(attr.Code, (method, attr));
}

while (ptr < prg.Count)
{
    var op = Decode(prg[ptr]);
    //Console.WriteLine($"Decoded {op}");
    ptr++;
    if (op.Code == Opcodes.Halt)
    {
        break;
    }
    var (method, meta) = cache[op.Code];
    var arguments = prg.GetRange(ptr, meta.ParamCount);
    //Console.WriteLine($"Executing with {string.Join(",", args)}");
    ptr += meta.ParamCount;
    method.Invoke(null, new object[] { op, arguments });
}

Opcode Decode(int opcodeRaw)
{
    var opcode = opcodeRaw % 100;
    var p0mode = opcodeRaw % 1000 / 100;
    var p1mode = opcodeRaw % 10000 / 1000;
    var p2mode = opcodeRaw % 100000 / 10000;

    return new Opcode
    {
        Code = (Opcodes)opcode,
        P0Mode = p0mode,
        P1Mode = p1mode,
        P2Mode = p2mode
    };
}

int Resolve(int value, int mode)
{
    return mode == 0 ? prg[value] : value;
}

#pragma warning disable CS8321 // Remove unused private members

[Opcode(Code = Opcodes.Add, ParamCount = 3)]
void Add(Opcode code, List<int> args)
{
    var value = Resolve(args[0], code.P0Mode) + Resolve(args[1], code.P1Mode);

    prg[args[2]] = value;
}

[Opcode(Code = Opcodes.Multiply, ParamCount = 3)]
void Multiply(Opcode code, List<int> args)
{
    var value = Resolve(args[0], code.P0Mode) * Resolve(args[1], code.P1Mode);

    prg[args[2]] = value;
}

[Opcode(Code = Opcodes.Input, ParamCount = 1)]
[SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Consistency")]
void Input(Opcode code, List<int> args)
{
    // prg[args[0]] = 1; // Part 1
    prg[args[0]] = 5; // Part 2
}

[Opcode(Code = Opcodes.Output, ParamCount = 1)]
[SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Consistency")]
void Output(Opcode code, List<int> args)
{
    Console.WriteLine(prg[args[0]]);
}

[Opcode(Code = Opcodes.JumpIfTrue, ParamCount = 2)]
void JumpIfTrue(Opcode code, List<int> args)
{
    if (Resolve(args[0], code.P0Mode) != 0)
    {
        ptr = Resolve(args[1], code.P1Mode);
    }
}

[Opcode(Code = Opcodes.JumpIfFalse, ParamCount = 2)]
void JumpIfFalse(Opcode code, List<int> args)
{
    if (Resolve(args[0], code.P0Mode) == 0)
    {
        ptr = Resolve(args[1], code.P1Mode);
    }
}

[Opcode(Code = Opcodes.LessThan, ParamCount = 3)]
void LessThan(Opcode code, List<int> args)
{
    var value = Resolve(args[0], code.P0Mode) < Resolve(args[1], code.P1Mode) ? 1 : 0;
    prg[args[2]] = value;
}

[Opcode(Code = Opcodes.Equals, ParamCount = 3)]
void Equals(Opcode code, List<int> args)
{
    var value = Resolve(args[0], code.P0Mode) == Resolve(args[1], code.P1Mode) ? 1 : 0;
    prg[args[2]] = value;
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
internal sealed class OpcodeAttribute : Attribute
{
    public Opcodes Code { get; set; }
    public int ParamCount { get; set; }
}

internal enum Opcodes
{
    Add = 1,
    Multiply = 2,
    Input = 3,
    Output = 4,
    JumpIfTrue = 5,
    JumpIfFalse = 6,
    LessThan = 7,
    Equals = 8,
    Halt = 99
}

internal struct Opcode
{
    public Opcodes Code;
    public int P0Mode;
    public int P1Mode;
    public int P2Mode;

    public override string ToString()
    {
        return Code.ToString();
    }
}