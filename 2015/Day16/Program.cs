using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

var rules = new Dictionary<string, int>
{
    { "children", 3 },
    { "cats", 7},
    { "samoyeds", 2},
    { "pomeranians", 3},
    { "akitas", 0},
    { "vizslas", 0},
    { "goldfish", 5},
    { "trees", 3},
    { "cars", 2},
    { "perfumes", 1},
};

var part1 = 0;
foreach (var row in File.ReadAllLines("input.txt"))
{
    var check = Regex.Match(row, @"(.+?): (.+)");
    var name = check.Groups[1].Value;
    var values = check.Groups[2].Value.Split(',').Select(v => v.Split(':')).ToDictionary(kv => kv[0].Trim(), kv => int.Parse(kv[1].Trim()));
    var rulesCount = values.Count(kv => rules[kv.Key] == kv.Value);
    if (rulesCount > part1)
    {
        part1 = rulesCount;
        Console.WriteLine(name);
    }
}