using AoC.Common;

Console.WriteLine("");

var grid = Grid.FromRows(File.ReadAllLines("input.txt"), c => c);

while (TiltNorthSouth(grid, -1))
{
}

var part1 = 0;

grid.ForEach((pos, v) =>
{
    part1 += v == 'O' ? (grid.Height - pos.y) : 0;
    return v;
});

Render.Result("Part 1", part1);

grid = Grid.FromRows(File.ReadAllLines("input.txt"), c => c);

var states = new Dictionary<string, int>();
var cycles = new Dictionary<string, int>();
var repeats = new HashSet<string>();

var cycle = 0;
var running = 0;
var i = 0;
while (running >= 0)
{
    Console.Write($"\r * Iteration: {i + 1:N0}. Cycle: {cycle}. Overflow: {running}");
    for (var stage = 0; stage < 4; stage++)
    {
        if (stage is 0 or 2)
        {
            while (TiltNorthSouth(grid, stage == 0 ? -1 : 1))
            {
            }
        }
        else
        {
            while (TiltEastWest(grid, stage == 1 ? -1 : 1))
            {
            }
        }
    }

    var state = string.Concat(grid);
    if (!states.TryGetValue(state, out var value))
    {
        states[state] = i;
    }
    else if (cycle == 0)
    {
        cycle = i - value;
        running = cycle;
        repeats.Add(state);
    }
    else
    {
        repeats.Add(state);
        running--;
    }
    i++;
}

foreach (var k in states.Keys.Where(k => !repeats.Contains(k)))
{
    states.Remove(k);
}

foreach (var state in states)
{
    var part2 = 0;

    var start = state.Value + 1;
    var offsetTarget = 1_000_000_000M - start;

    if (decimal.Remainder(offsetTarget, cycle) != 0)
    {
        continue;
    }

    grid = new Grid<char>(grid.Width, grid.Height, state.Key);
    grid.ForEach((pos, v) =>
    {
        part2 += v == 'O' ? (grid.Height - pos.y) : 0;
        return v;
    });

    Console.Write($"\r{new string(' ', Console.WindowWidth - 5)}\r");
    Render.Result("Part 2", part2);
}

static bool TiltNorthSouth(Grid<char> grid, int delta)
{
    var moved = false;
    foreach (var row in Enumerable.Range(1, grid.Height - 1).Select(n => delta == -1 ? n : grid.Height - n - 1).ToList())
    {
        for (var col = 0; col < grid.Width; col++)
        {
            if (grid.GetValue(col, row) == 'O' && grid.GetValue(col, row + delta) == '.')
            {
                grid.SetValue(col, row + delta, 'O');
                grid.SetValue(col, row, '.');
                moved = true;
            }
        }
    }

    return moved;
}

static bool TiltEastWest(Grid<char> grid, int delta)
{
    var moved = false;
    foreach (var col in Enumerable.Range(1, grid.Width - 1).Select(n => delta == -1 ? n : grid.Width - n - 1).ToList())
    {
        for (var row = 0; row < grid.Width; row++)
        {
            if (grid.GetValue(col, row) == 'O' && grid.GetValue(col + delta, row) == '.')
            {
                grid.SetValue(col + delta, row, 'O');
                grid.SetValue(col, row, '.');
                moved = true;
            }
        }
    }

    return moved;
}