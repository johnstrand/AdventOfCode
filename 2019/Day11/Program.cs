using AoC.Common;

var facing = 0;
var pos = (x: 0, y: 0);
var panels = new Dictionary<(int x, int y), int>();

var deltas = new Dictionary<int, (int x, int y)>
{
    [0] = (0, -1),
    [1] = (1, 0),
    [2] = (0, 1),
    [3] = (-1, 0),
};

var part2 = true;

if (part2)
{
    panels[(0, 0)] = 1;
}

var mode = Mode.Paint;

var computer = IntCode.Parse(File.ReadAllText("input.txt"));

computer.SetInput(() => panels.TryGetValue(pos, out var v) && v == 1 ? 1 : 0);

computer.SetOutput(v =>
{
    if (!panels.ContainsKey(pos))
    {
        panels[pos] = 0;
    }

    if (mode == Mode.Paint)
    {
        if (part2)
        {
            Console.SetCursorPosition(pos.x, pos.y);
            Console.Write(v == 1 ? '#' : '.');
        }
        panels[pos] = (int)v;
        mode = Mode.Move;
    }
    else
    {
        facing = (facing + (v == 0 ? -1 : 1)) % 4;
        if (facing == -1)
        {
            facing = 3;
        }

        pos = (x: pos.x + deltas[facing].x, y: pos.y + deltas[facing].y);
        if (part2)
        {
            Console.SetCursorPosition(pos.x, pos.y);
            Console.Write(facing switch
            {
                0 => '^',
                1 => '>',
                2 => 'v',
                _ => '<'
            });
        }
        mode = Mode.Paint;
    }
});

computer.Run(false);
Console.WriteLine();

if (!part2)
{
    Console.WriteLine($"Part 1: {panels.Keys.Count}");
}

internal enum Mode
{
    Move,
    Paint
}
