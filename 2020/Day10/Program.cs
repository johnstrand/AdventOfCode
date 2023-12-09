using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

Dictionary<long, long> paths = [];

var rows = File.ReadAllLines("input.txt").Select(long.Parse).ToList();
rows.Add(0);
rows.Add(rows.Max() + 3);
rows.Sort();
var diffs = new Dictionary<long, long>();

for (var i = 0; i < rows.Count - 1; i++)
{
    var diff = rows[i + 1] - rows[i];
    diffs[diff] = (diffs.ContainsKey(diff) ? diffs[diff] : 0) + 1;
}

Console.WriteLine($"Part 1: {diffs[1] * diffs[3]}");

var target = rows.Max();

Console.WriteLine($"Part 2: {Search(0, target, rows)}");

long Search(long current, long target, List<long> options)
{
    if (current == target)
    {
        return 1;
    }

    var opts = options.Where(o => o > current && o <= current + 3).ToList();
    var sum = 0L;
    foreach (var opt in opts)
    {
        if (!paths.ContainsKey(opt))
        {
            paths[opt] = Search(opt, target, options.Where(o => o > opt).ToList());
        }
        sum += paths[opt];
    }
    return sum;
}
