using AoC.Common;

var grid = new Grid<char>(Input.ReadActual(), c => c);

var part1 = 0;
var part2 = 0;

var first = true;

while (true)
{
    var toRemove = new List<(int x, int y)>();

    foreach (var (x, y) in grid.Coordinates)
    {
        if (grid.GetValue(x, y) != '@')
        {
            continue;
        }

        var count = grid.GetAdjacent(x, y).Count(p => grid.GetValue(p) == '@');

        if (count < 4)
        {
            if (first)
            {
                part1++;
            }

            toRemove.Add((x, y));
        }
    }

    if (toRemove.Count == 0)
    {
        break;
    }

    part2 += toRemove.Count;

    foreach (var (x, y) in toRemove)
    {
        grid.SetValue(x, y, '.');
    }

    first = false;
}

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");
