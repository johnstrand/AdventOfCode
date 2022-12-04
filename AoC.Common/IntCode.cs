using System.Collections;

namespace AoC.Common;

public enum Operation
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

public enum Mode
{
    Position,
    Immediate
}

internal record Instruction(Operation Opcode, Mode P0, Mode P1, Mode P2)
{
    public static Instruction Decode(int rawCode)
    {
        var opcode = rawCode % 100;
        var p0mode = rawCode % 1000 / 100;
        var p1mode = rawCode % 10000 / 1000;
        var p2mode = rawCode % 100000 / 10000;

        return new(
            (Operation)opcode,
            (Mode)p0mode,
            (Mode)p1mode,
            (Mode)p2mode);
    }

    public override string ToString()
    {
        return $"{Opcode} ({P0} , {P1} , {P2} )";
    }
}

public class IntCode
{
    private int _ptr;

    private readonly List<int> _mem;
    private readonly List<int> _original;
    private readonly List<int> _output = new();

    private bool _halted;
    private bool _debug;

    private readonly Dictionary<Operation, (int length, Action<Instruction, int[]> action)> _instruction = new();

    private Func<int> valueSource = () => 0;

    public static IntCode Parse(string program)
    {
        return new(program.Split(','));
    }

    public void SetInput(Func<int> valueSource)
    {
        this.valueSource = valueSource;
    }

    public IntCode(string[] program)
    {
        _mem = program.Select(int.Parse).ToList();
        _original = new(_mem);

        _instruction[Operation.Add] = (4, (instr, args) => Set(
            args[2],
            Get(args[0], instr.P0) + Get(args[1], instr.P1)));

        _instruction[Operation.Multiply] = (4, (instr, args) => Set(
            args[2],
            Get(args[0], instr.P0) * Get(args[1], instr.P1)));

        _instruction[Operation.Input] = (2, (_, args) =>
            Set(args[0], valueSource()));

        _instruction[Operation.Output] = (2, (instr, args) =>
            _output.Add(Get(args[0], instr.P0)));

        _instruction[Operation.JumpIfTrue] = (
            3,
            (instr, args) =>
            {
                if (Get(args[0], instr.P0) != 0)
                {
                    _ptr = Get(args[1], instr.P1);
                }
            }
        );

        _instruction[Operation.JumpIfFalse] = (
            3,
            (instr, args) =>
            {
                if (Get(args[0], instr.P0) == 0)
                {
                    _ptr = Get(args[1], instr.P1);
                }
            }
        );

        _instruction[Operation.LessThan] = (
            4,
            (instr, args) => Set(args[2], Get(args[0], instr.P0) < Get(args[1], instr.P1) ? 1 : 0)
        );

        _instruction[Operation.Equals] = (
            4,
            (instr, args) => Set(args[2], Get(args[0], instr.P0) == Get(args[1], instr.P1) ? 1 : 0)
        );

        _instruction[Operation.Halt] = (1, (_, _) => _halted = true);
    }

    public int Resolve(int value, Mode mode)
    {
        return mode switch
        {
            Mode.Position => _mem[value],
            Mode.Immediate => value,
            _ => throw new($"Unknown mode: {mode}")
        };
    }

    public void Reset()
    {
        _mem.Clear();
        _mem.AddRange(_original);
        _ptr = 0;
        _halted = false;
    }

    public void Run(bool debug = false)
    {
        _debug = debug;
        while (Step()) { }
        if (_output.Count > 0)
        {
            Console.WriteLine($"OUT >> {string.Concat(_output)}");
            _output.Clear();
        }
    }

    private bool Step()
    {
        if (_halted)
        {
            return false;
        }

        var instruction = Debug(Instruction.Decode(_mem[_ptr]), "Decode");

        var (length, action) = _instruction[instruction.Opcode];

        var ptrBefore = _ptr;
        action(instruction, Debug(_mem.GetRange(_ptr, length).Skip(1).ToArray(), "Args"));

        if (ptrBefore == _ptr)
        {
            _ptr += length;
        }

        Debug(_ptr, "Pointer");

        return !_halted;
    }

    public int Set(int ptr, int value)
    {
        return Debug(_mem[ptr] = value, $"Set {ptr}");
    }

    public int Get(int ptr, Mode mode)
    {
        return Debug(Resolve(ptr, mode), $"Get {ptr} in mode {mode}");
    }

    private T Debug<T>(T value, string msg)
    {
        if (_debug)
        {
            if (value is not string && value is IEnumerable list)
            {
                Console.WriteLine($"[DBG] {msg} :: {string.Join(", ", list.Cast<object>())}");
            }
            else
            {
                Console.WriteLine($"[DBG] {msg} :: {value}");
            }
        }

        return value;
    }
}
