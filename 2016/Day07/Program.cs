using System.Text;
using System.Text.RegularExpressions;

var valid = 0;
foreach (var row in File.ReadAllLines("input.txt"))
{
    GetSegments(row, out var regular, out var hyper);
    var isValid = regular.Any(HasAbba) && !hyper.Any(HasAbba);
    if (isValid)
    {
        valid++;
    }
}

Console.WriteLine($"Part 1: {valid}");

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
