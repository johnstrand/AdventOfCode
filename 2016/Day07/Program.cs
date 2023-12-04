using System.Text;

using AoC.Common;

var part1 = 0;
var part2 = 0;
foreach (var row in File.ReadAllLines("input.txt"))
{
    GetSegments(row, out var regular, out var hyper);
    var isValid = regular.Any(HasAbba) && !hyper.Any(HasAbba);
    if (isValid)
    {
        part1++;
    }

    if (GetABA(regular, out var aba) && aba.Any(x => hyper.Any(y => HasBAB(y, x))))
    {
        part2++;
    }
}

Render.Result("Part 1", part1);
Render.Result("Part 2", part2);

static bool GetABA(List<string> values, out List<string> aba)
{
    aba = new List<string>();
    foreach (var value in values)
    {
        for (var i = 0; i < value.Length - 2; i++)
        {
            if (value[i] == value[i + 2] && value[i] != value[i + 1])
            {
                aba.Add(value.Substring(i, 3));
            }
        }
    }

    return aba.Count > 0;
}

static bool HasBAB(string s, string aba)
{
    var bab = new string(new[] { aba[1], aba[0], aba[1] });
    return s.Contains(bab);
}

static void GetSegments(string value, out List<string> regular, out List<string> hyper)
{
    regular = new List<string>();
    hyper = new List<string>();
    var buffer = new StringBuilder();
    foreach (var c in value)
    {
        if (c == '[')
        {
            regular.Add(buffer.ToString());
            buffer.Clear();
        }
        else if (c == ']')
        {
            hyper.Add(buffer.ToString());
            buffer.Clear();
        }
        else
        {
            buffer.Append(c);
        }
    }
    if (buffer.Length > 0)
    {
        regular.Add(buffer.ToString());
    }
}

static bool HasAbba(string s)
{
    for (var i = 0; i < s.Length - 3; i++)
    {
        if (s[i] == s[i + 3] && s[i + 1] == s[i + 2] && s[i] != s[i + 1])
        {
            return true;
        }
    }

    return false;
}
