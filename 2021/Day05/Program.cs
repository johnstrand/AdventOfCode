// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

var grid = new Dictionary<(int x, int y), int>();
foreach (var part in new[] { 1, 2 })
{
    grid.Clear();
    foreach (var line in File.ReadAllLines("input.txt"))
    {
        Parse(line, out var start, out var end);

        var delta = GetDelta(start, end);

        if (part == 1 && delta.x != 0 && delta.y != 0)
        {
            continue;
        }

        while (start != end)
        {
            //Console.WriteLine(start);
            Count(start);
            start = Add(start, delta);
        }
        //Console.WriteLine(end);
        Count(end);
    }

    Console.WriteLine($"Part {part}: {grid.Count(kv => kv.Value > 1)}");
}

void Count((int x, int y) pos)
{
    if (!grid.TryGetValue(pos, out var value))
    {
        value = 0;
        grid[pos] = value;
    }

    grid[pos] = ++value;
}

(int x, int y) Add((int x, int y) a, (int x, int y) b)
{
    return (
        a.x + b.x,
        a.y + b.y
    );
}

(int x, int y) GetDelta((int x, int y) from, (int x, int y) to)
{
    return (
        Normalize(to.x - from.x),
        Normalize(to.y - from.y)
    );
}

int Normalize(int v)
{
    return v == 0 ? v : v / Math.Abs(v);
}

void Parse(string row, out (int x, int y) start, out (int x, int y) end)
{
    var m = Regex.Match(row, @"^(\d+),(\d+) -> (\d+),(\d+)$");
    start = (
        x: int.Parse(m.Groups[1].Value),
        y: int.Parse(m.Groups[2].Value));
    end = (
        x: int.Parse(m.Groups[3].Value),
        y: int.Parse(m.Groups[4].Value));
}
