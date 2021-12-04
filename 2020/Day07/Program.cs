using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

var rules = new Dictionary<string, List<(string name, int count)>>();

var pattern = @"(.+?) bags contain (.+)\.";

foreach (var row in File.ReadAllLines("input.txt"))
{
    var match = Regex.Match(row, pattern);
    var name = match.Groups[1].Value;
    var values = match.Groups[2].Value;
    if (values.Contains("no other"))
    {
        rules[name] = new List<(string name, int count)>();
        continue;
    }

    var v = Regex.Matches(values, @"(\d+) (.+?) bags?");
    rules[name] = v.Select(m => (m.Groups[2].Value, int.Parse(m.Groups[1].Value))).ToList();
}

var part1 = 0;
foreach (var rule in rules)
{
    if (CanContain("shiny gold", rule.Value))
    {
        Console.WriteLine($"{rule.Key} can contain shiny gold");
        part1++;
    }
}

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {Count("shiny gold")}");

int Count(string name)
{
    return rules[name].Sum(r => r.count + (r.count * Count(r.name)));
}

bool CanContain(string name, List<(string name, int count)> rule)
{
    return rule.Any(r => r.name == name || CanContain(name, rules[r.name]));
}
