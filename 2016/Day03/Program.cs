﻿using System;
using System.IO;
using System.Linq;

var sum = 0;
foreach (var row in File.ReadAllLines("input.txt"))
{
    var tri = row.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).OrderBy(x => x).ToList();
    if (tri.Take(2).Sum() > tri.Last())
    {
        sum++;
    }
}

Console.WriteLine($"Part 1: {sum}");

sum = 0;
var rows = File.ReadAllLines("input.txt").Select(row => row.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()).ToList();
for (var i = 0; i < rows.Count; i += 3)
{
    for (var offset = 0; offset < 3; offset++)
    {
        var tri = new[] { rows[i][offset], rows[i + 1][offset], rows[i + 2][offset] }.OrderBy(x => x).ToList();
        if (tri.Take(2).Sum() > tri.Last())
        {
            sum++;
        }
    }
}

Console.WriteLine($"Part 2: {sum}");
