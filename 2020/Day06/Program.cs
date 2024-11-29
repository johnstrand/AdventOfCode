using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var currentGroup = new Dictionary<char, int>();
var part1 = 0;
var part2 = 0;
var groupCount = 0;
foreach (var row in File.ReadAllLines("input.txt"))
{
    if (row?.Length == 0)
    {
        part1 += currentGroup.Count;
        part2 += currentGroup.Values.Count(v => v == groupCount);
        groupCount = 0;
        currentGroup.Clear();
    }
    else
    {
        groupCount++;
        foreach (var c in row)
        {
            if (currentGroup.TryGetValue(c, out var value))
            {
                currentGroup[c] = ++value;
            }
            else
            {
                currentGroup.Add(c, 1);
            }
        }
    }
}

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");