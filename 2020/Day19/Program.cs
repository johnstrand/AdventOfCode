﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

Dictionary<int, string> rules;

// TODO: Part 2
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

    var re = new Regex("^" + Render(0) + "$");
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

string Render(int ruleId)
{
    var rule = rules[ruleId];
    if (char.IsLetter(rule[0]))
    {
        return rule[0].ToString();
    }
    return "(" + string.Join("|", rule.Split('|').Select(r => r.Trim()).Select(expr =>
    {
        return string.Join("", expr.Split(' ').Select(int.Parse).Select(Render));
    })) + ")";
}

