using AoC.Common;

static string Capture(string value, int targetLen)
{
    if (value.Length == targetLen)
    {
        return value;
    }

    for (var i = 0; i < value.Length - 1; i++)
    {
        if (value[i] < value[i + 1])
        {
            var valueSpan = value.AsSpan();
            return Capture(string.Concat(valueSpan[0..i], valueSpan[(i + 1)..]), targetLen);
        }
    }

    return Capture(value[0..^1], targetLen);
}

var part1 = 0;
var part2 = 0L;
foreach (var bank in Input.ReadActual())
{
    part1 += int.Parse(Capture(bank, 2));
    part2 += long.Parse(Capture(bank, 12));
}

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");
