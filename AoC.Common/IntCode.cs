namespace AoC.Common;

public class IntCode
{
    private int _ptr;

    private readonly List<int> _mem;
    private readonly List<int> _original;

    private bool _halted;

    private readonly Dictionary<int, (int length, Action<int[]> action)> _instruction = new();

    public static IntCode Parse(string program)
    {
        return new(program.Split(','));
    }

    public IntCode(string[] program)
    {
        _mem = program.Select(int.Parse).ToList();
        _original = new(_mem);
        _instruction[1] = (4, args => Set(args[2], Get(args[0]) + Get(args[1])));
        _instruction[2] = (4, args => Set(args[2], Get(args[0]) * Get(args[1])));

        _instruction[99] = (1, _ => _halted = true);
    }

    public IntCode Instruction(int opcode, int length, Action<int[]> action)
    {
        _instruction[opcode] = (length, action);
        return this;
    }

    public void Reset()
    {
        _mem.Clear();
        _mem.AddRange(_original);
        _ptr = 0;
        _halted = false;
    }

    public void Run()
    {
        while (Step()) { }
    }

    public bool Step()
    {
        if (_halted)
        {
            return false;
        }

        var opcode = _mem[_ptr];

        var (length, action) = _instruction[opcode];

        action(_mem.GetRange(_ptr, length).Skip(1).ToArray());

        _ptr += length;

        return !_halted;
    }

    public int Set(int ptr, int value)
    {
        return _mem[ptr] = value;
    }

    public int Get(int ptr)
    {
        return _mem[ptr];
    }
}
