using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

Dictionary<int, string> rules;

// TODO: Rewrite without cheating
foreach (var part in new[] { 1, 2 })
{
    rules = new Dictionary<int, string>();

    using var reader = new StreamReader("input.txt");
    while (true)
    {
        var next = reader.ReadLine();
        if (string.IsNullOrWhiteSpace(next))
        {
            break;
        }
        var parts = next.Split(':');
        rules[int.Parse(parts[0])] = parts[1].Trim().Trim('"');
    }

    if (part == 2)
    {
        rules[8] = "42 | 42 8";
        rules[11] = "42 31 | 42 11 31";
    }

    Console.Write("Generating regular expresssion...");
    var re = new Regex("^" + Render(0) + "$");
    Console.WriteLine("done");
    var matching = 0;
    while (!reader.EndOfStream)
    {
        var text = reader.ReadLine();
        if (re.IsMatch(text))
        {
            matching++;
        }
    }
    Console.WriteLine($"Part {part}: {matching}");
}

string Render(int ruleId, int depth = 0)
{
    if (depth > 20)
    {
        return "";
    }
    var rule = rules[ruleId];
    return char.IsLetter(rule[0])
        ? rule[0].ToString()
        : "(" + string.Join("|", rule.Split('|').Select(r => r.Trim()).Select(expr => string.Concat(expr.Split(' ').Select(int.Parse).Select(v => Render(v, depth + 1))))) + ")";
}
