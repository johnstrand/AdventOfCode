using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var pairings = File.ReadAllLines("input.txt").Select(row => row.TrimEnd('.').Split(' ')).ToDictionary(r => $"{r.First()}_{r.Last()}", r => int.Parse(r[3]) * (r[2] == "gain" ? 1 : -1));
var participants = new HashSet<string>(pairings.Keys.Select(k => k.Split('_').First())).ToList();

var part1 = 0;
foreach (var setup in GetPermutations(participants))
{
    var setupPoints = 0;
    for (var i = 0; i < setup.Count; i++)
    {
        var participant = setup[i];
        var right = setup[i == 0 ? setup.Count - 1 : i - 1];
        var left = setup[(i + 1) % setup.Count];
        var rightScore = pairings[$"{participant}_{right}"];
        var leftScore = pairings[$"{participant}_{left}"];

        setupPoints += (rightScore + leftScore);
    }
    if (setupPoints > part1)
    {
        part1 = setupPoints;
        Console.WriteLine($"{string.Join(", ", setup)}: {setupPoints}");
    }
}
Console.WriteLine($"Part 1: {part1}");

IEnumerable<List<string>> GetPermutations(List<string> source)
{
    var limit = Fact(source.Count);
    for (var j = 0; j < limit; j++)
    {
        for (var i = 0; i < source.Count - 1; i++)
        {
            yield return source.ToList();
            var temp = source[i + 1];
            source[i + 1] = source[i];
            source[i] = temp;
        }
    }
}

int Fact(int n)
{
    return n == 0 ? 1 : n * Fact(n - 1);
}