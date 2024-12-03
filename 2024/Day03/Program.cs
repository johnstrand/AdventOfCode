using System.Text.RegularExpressions;

using AoC.Common;

var expr = string.Join(" ", Input.ReadActual());

var part1 = Regex
    .Matches(expr, @"mul\((\d+),(\d+)\)")
    .Select(m => int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value))
    .Sum();

Console.WriteLine($"Part 1: {part1}");

expr = string.Join(" ", Input.ReadActual());

var enabled = true;
var part2 = Regex
    .Matches(expr, @"(do(n't)*\(\)|mul\((\d+),(\d+)\))")
    .Select(m =>
    {
        if (m.Value == "do()")
        {
            enabled = true;
            return 0;
        }
        else if (m.Value == "don't()")
        {
            enabled = false;
            return 0;
        }
        else if (!enabled)
        {
            return 0;
        }

        return int.Parse(m.Groups[3].Value) * int.Parse(m.Groups[4].Value);
    })
    .Sum();

Console.WriteLine($"Part 2: {part2}");
