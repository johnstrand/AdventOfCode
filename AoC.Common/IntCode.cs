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
    RelBase = 9,
    Halt = 99
}

public enum Mode
{
    Position,
    Immediate,
    Relative
}

internal record Instruction(Operation Opcode, Mode P0, Mode P1, Mode P2)
{
    public static Instruction Decode(long rawCode)
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
        return Opcode.ToString();
    }
}

public class IntCode
{
    private long _ptr;
    private long _relBase;

    private readonly Dictionary<long, long> _mem;
    private readonly Dictionary<long, long> _original;
    private readonly List<long> _output = new();

    private bool _halted;
    private bool _debug;

    private readonly Dictionary<Operation, (int length, Action<Instruction, long[]> action)> _instruction = new();

    private Func<long> _valueSource = () => 0;
    private Action<long>? _outputHandler;

    public static IntCode Parse(string program)
    {
        return new(program.Split(','));
    }

    public void SetInput(Func<long> valueSource)
    {
        _valueSource = valueSource;
    }

    public void SetOutput(Action<long> handler)
    {
        _outputHandler = handler;
    }

    public IntCode(string[] program)
    {
        _mem = program.Select(long.Parse).Select((v, i) => (v, i)).ToDictionary(x => (long)x.i, x => x.v);
        _original = new(_mem);

        _instruction[Operation.Add] = (4, (instr, args) => Set(
            args[2],
            Get(args[0], instr.P0) + Get(args[1], instr.P1), instr.P2 == Mode.Relative));

        _instruction[Operation.Multiply] = (4, (instr, args) => Set(
            args[2],
            Get(args[0], instr.P0) * Get(args[1], instr.P1), instr.P2 == Mode.Relative));

        _instruction[Operation.Input] = (2, (instr, args) =>
            Set(args[0], _valueSource(), instr.P0 == Mode.Relative));

        _instruction[Operation.Output] = (
            2,
            (instr, args) =>
            {
                if (_outputHandler != null)
                {
                    _outputHandler(Get(args[0], instr.P0));
                }
                else
                {
                    _output.Add(Get(args[0], instr.P0));
                }
            }
        );

        _instruction[Operation.JumpIfTrue] = (
            3,
            (instr, args) =>
            {
                if (Get(args[0], instr.P0) != 0)
                {
                    _ptr = (int)Get(args[1], instr.P1);
                }
            }
        );

        _instruction[Operation.JumpIfFalse] = (
            3,
            (instr, args) =>
            {
                if (Get(args[0], instr.P0) == 0)
                {
                    _ptr = (int)Get(args[1], instr.P1);
                }
            }
        );

        _instruction[Operation.LessThan] = (
            4,
            (instr, args) => Set(args[2], Get(args[0], instr.P0) < Get(args[1], instr.P1) ? 1 : 0, instr.P2 == Mode.Relative)
        );

        _instruction[Operation.Equals] = (
            4,
            (instr, args) => Set(args[2], Get(args[0], instr.P0) == Get(args[1], instr.P1) ? 1 : 0, instr.P2 == Mode.Relative)
        );

        _instruction[Operation.RelBase] = (
            2,
            (instr, args) => _relBase += (int)Get(args[0], instr.P0));

        _instruction[Operation.Halt] = (1, (_, _) => _halted = true);
    }

    public long Resolve(long value, Mode mode)
    {
        return mode switch
        {
            Mode.Position => _mem.GetValueOrDefault(Debug((int)value, "Position")),
            Mode.Relative => _mem.GetValueOrDefault(Debug((int)(_relBase + value), $"Relative {_relBase} + {value}")),
            Mode.Immediate => value,
            _ => throw new($"Unknown mode: {mode}")
        };
    }

    public void Reset()
    {
        _mem.Clear();
        foreach (var item in _original)
        {
            _mem[item.Key] = item.Value;
        }
        _ptr = 0;
        _relBase = 0;
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

        if (_debug)
        {
            for (var i = _ptr; i < Math.Min(_ptr + 8, _mem.Count); i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("[");
                Console.ForegroundColor = i == _ptr ? ConsoleColor.White : ConsoleColor.Gray;
                Console.Write($"{_mem[(int)i]}");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("]");
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        var instruction = Debug(Instruction.Decode(Debug(_mem[(int)_ptr], "Instruction")), "Decode");

        var (length, action) = _instruction[instruction.Opcode];

        var ptrBefore = _ptr;
        var args = _mem.GetRange((int)_ptr, length).Skip(1).ToArray();

        Debug(args.Select((a, i) => $"{a} ({(i == 0 ? instruction.P0 : i == 1 ? instruction.P1 : instruction.P2)})").ToList(), "Args");

        action(instruction, args);

        if (ptrBefore == _ptr)
        {
            _ptr += length;
        }

        Debug(_ptr, "Pointer");
        Debug(_relBase, "Relative");

        return !_halted;
    }

    public long Set(long ptr, long value, bool relative)
    {
        var target = (int)((relative ? _relBase : 0) + ptr);
        return Debug(_mem[target] = value, $"Set {target} ({(relative ? "Relative" : "Absolute")})");
    }

    public long Get(long ptr, Mode mode)
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

internal static class IntCodeExtensions
{
    public static IEnumerable<long> GetRange(this Dictionary<long, long> dict, long index, long count)
    {
        while (count-- > 0)
        {
            yield return dict.GetValueOrDefault(index++);
        }
    }
}
