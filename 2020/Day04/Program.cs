using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

var data = new Queue<string>(File.ReadLines("input.txt"));
var passport = new Dictionary<string, string>();
var part1 = 0;
var part2 = 0;

while (data.Count > 0)
{
    var next = data.Dequeue();
    if (next?.Length == 0)
    {
        if (Valid1(passport))
        {
            part1++;
        }

        if (Valid2(passport))
        {
            part2++;
        }

        passport.Clear();
    }
    else
    {
        next.Split(' ').Select(item => item.Split(':')).ToList().ForEach(pair => passport.Add(pair[0], pair[1]));
    }
}

if (Valid1(passport))
{
    part1++;
}

if (Valid2(passport))
{
    part2++;
}

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

bool Valid1(Dictionary<string, string> passport)
{
    return passport.Count == 8 || (passport.Count == 7 && !passport.ContainsKey("cid"));
}

bool Valid2(Dictionary<string, string> passport)
{
    return
        TryValidate(passport, "byr", v => int.TryParse(v, out var y) && y >= 1920 && y <= 2002) &&
        TryValidate(passport, "iyr", v => int.TryParse(v, out var y) && y >= 2010 && y <= 2020) &&
        TryValidate(passport, "eyr", v => int.TryParse(v, out var y) && y >= 2020 && y <= 2030) &&
        TryValidate(passport, "hgt", ValidHeight) &&
        TryValidate(passport, "hcl", v => Regex.IsMatch(v, "^#[0-9a-f]{6}$")) &&
        TryValidate(passport, "ecl", ValidColor) &&
        TryValidate(passport, "pid", v => v.Length == 9 && v.All(char.IsDigit));
}

bool ValidHeight(string h)
{
    if (h.EndsWith("cm"))
    {
        return int.TryParse(h[0..^2], out var v) && v >= 150 && v <= 193;
    }
    else
    {
        return h.EndsWith("in") && int.TryParse(h[0..^2], out var v) && v >= 59 && v <= 76;
    }
}

bool ValidColor(string h)
{
    var clr = new HashSet<string>
    {
        "amb", "blu", "brn", "gry", "grn", "hzl", "oth"
    };

    return clr.Contains(h);
}

bool TryValidate(Dictionary<string, string> passport, string key, Func<string, bool> validator)
{
    return passport.TryGetValue(key, out var v) && validator(v);
}
