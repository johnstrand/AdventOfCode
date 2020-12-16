using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

var pattern = @"(\w+): (\d+)-(\d+) or (\d+)-(\d+)";
var rules = new Dictionary<string, List<int>>();
using var reader = new StreamReader("input-test.txt");
string row;
while (!string.IsNullOrWhiteSpace(row = reader.ReadLine()))
{
    var match = Regex.Match(row, pattern);
    rules[match.Groups[1].Value] = new List<int>
    {
        int.Parse(match.Groups[2].Value),
        int.Parse(match.Groups[3].Value),
        int.Parse(match.Groups[4].Value),
        int.Parse(match.Groups[5].Value),
    };
}


// Skip header
reader.ReadLine();

// Skip my ticket
var ticket = reader.ReadLine().Split(',').Select(int.Parse).ToList();

var candidates = rules.ToDictionary(r => r.Key, _ => Enumerable.Range(0, ticket.Count).ToList());

// Skip whitespace
reader.ReadLine();

// Skip header
reader.ReadLine();

var checksum = 0;
while (!reader.EndOfStream)
{
    var values = reader.ReadLine().Split(',').Select(int.Parse).ToList();
    var valueIndex = 0;
    foreach (var value in values)
    {
        if (rules.Any(rule => InRange(value, rule.Value)))
        {
            continue;
        }

        foreach (var rule in rules)
        {
            if (!InRange(value, rule.Value))
            {
                Console.WriteLine($"Removed index {valueIndex} from rule {rule.Key}");
                Console.WriteLine($"Matched {value} with {string.Join(", ", rule.Value)}");
                candidates[rule.Key].Remove(valueIndex);
                Console.WriteLine($"{rule.Key} remaining: {string.Join(", ", candidates[rule.Key])}");
            }
        }
        valueIndex++;
        checksum += value;
    }
}

Console.WriteLine($"Part 1: {checksum}");

bool InRange(int v, List<int> ranges)
{
    return (v >= ranges[0] && v <= ranges[1]) || (v >= ranges[2] && v <= ranges[3]);
}
