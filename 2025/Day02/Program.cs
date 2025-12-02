using System.Text.RegularExpressions;

using AoC.Common;

var part1 = 0L;
var part2 = 0L;
foreach (var pair in Input.ReadActual().SelectMany(r => r.Split(',')))
{
    var rangeParts = pair.Split('-').Select(long.Parse).ToArray();
    Console.WriteLine($"Range: {rangeParts[0]} - {rangeParts[1]}");
    for (long i = rangeParts[0]; i <= rangeParts[1]; i++)
    {
        var v = i.ToString();

        if ((v.Length & 1) == 0 && v[0..(v.Length / 2)] == v[(v.Length / 2)..])
        {
            part1 += i;
        }

        if (Regex.Match(v, @"^(\d+)(\1)+$").Success)
        {
            part2 += i;
        }
    }
}

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");