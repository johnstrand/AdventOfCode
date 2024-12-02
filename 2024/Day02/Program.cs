using AoC.Common;

var part1 = 0;
var part2 = 0;

static bool IsSafe(int[] levels)
{
    var expected = levels[0].CompareTo(levels[1]);

    for (var i = 0; i < levels.Length - 1; i++)
    {
        if (levels[i].CompareTo(levels[i + 1]) != expected || Math.Abs(levels[i] - levels[i + 1]) is < 1 or > 3)
        {
            return false;
        }
    }

    return true;
}

static bool CanBeSafe(int[] levels)
{
    for (var i = 0; i < levels.Length; i++)
    {
        var newArray = levels.Where((_, ix) => ix != i).ToArray();

        if (IsSafe(newArray))
        {
            return true;
        }
    }

    return false;
}

foreach (var report in Input.ReadActual())
{
    var reportValues = report.Split(' ').ToNumbers32().ToArray();
    if (IsSafe(reportValues))
    {
        part1++;
        part2++;
    }
    else if (CanBeSafe(reportValues))
    {
        part2++;
    }
}

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");