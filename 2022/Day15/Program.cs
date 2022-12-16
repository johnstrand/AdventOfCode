using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using AoC.Common;

static int Dist(int x1, int y1, int x2, int y2) => Math.Abs(x1 - x2) + Math.Abs(y1 - y2);

const long bounds = 4_000_000;
const int rowIndex = 2_000_000;

var rows = File.ReadAllLines("input.txt").ToList();

var index = 0;

var skipLists = new Dictionary<int, List<(int from, int to)>>();

foreach (var row in rows)
{
    index++;
    Console.Write($"Parsing {index} / {rows.Count}\r");
    var parsed = Regex.Match(row, @"Sensor at x=(\d+), y=(\d+): closest beacon is at x=(-?\d+), y=(-?\d+)");

    var sx = int.Parse(parsed.Groups[1].Value);
    var sy = int.Parse(parsed.Groups[2].Value);

    var dx = int.Parse(parsed.Groups[3].Value);
    var dy = int.Parse(parsed.Groups[4].Value);

    var envelope = Dist(sx, sy, dx, dy);

    for (var r = -envelope; r <= envelope; r++)
    {
        var d = envelope - Math.Abs(r);

        var x1 = sx - d;
        var x2 = sx + d;

        var y = sy + r;

        if (y < 0 || y > bounds)
        {
            continue;
        }

        if (!skipLists.ContainsKey(y))
        {
            skipLists[y] = new();
        }

        skipLists[y].Add((x1, x2));
    }
}

Console.WriteLine();

var p1 = skipLists[rowIndex];

var part1 = p1.Max(n => n.to) - p1.Min(n => n.from);

Console.WriteLine();
Console.WriteLine($"Part 1: {part1}");

Parallel.ForEach(skipLists, (kv, state) =>
{
    var y = kv.Key;
    for (var x = 0; x < bounds; x++)
    {
        var skips = kv.Value.Where(n => x >= n.from && x <= n.to).ToList();

        if (skips.Count == 0)
        {
            Console.WriteLine($"Part 2: {4_000_000L * x + y}");
            state.Break();
            return;
        }

        skips.ForEach(v => kv.Value.Remove(v));
        x = skips.Max(n => n.to);
    }
});

