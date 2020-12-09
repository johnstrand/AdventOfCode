using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var data = File.ReadAllLines("input.txt").Select(long.Parse).ToList();

var preamble = 25;
var target = 0L;

for (var i = preamble; i < data.Count; i++)
{
    if (!MatchSum(data[i], data.Skip(i - preamble).Take(preamble).ToList()))
    {
        Console.WriteLine($"Part 1: {data[i]}");
        target = data[i];
        break;
    }
}

for (var i = 0; i < data.Count; i++)
{
    for (var ln = 1; ln < data.Count; ln++)
    {
        var range = data.Skip(i).Take(ln).ToList();
        if (range.Distinct().Count() != range.Count)
        {
            continue;
        }
        var sum = range.Sum();
        if (sum == target)
        {
            Console.WriteLine($"Part 2: {range.Min() + range.Max()}");
            break;
        }
        if (sum > target)
        {
            break;
        }
    }
}

bool MatchSum(long v, List<long> values)
{
    for (var i = 0; i < values.Count - 1; i++)
    {
        for (var j = i + 1; j < values.Count; j++)
        {
            if (values[i] != values[j] && values[i] + values[j] == v)
            {
                return true;
            }
        }
    }

    return false;
}