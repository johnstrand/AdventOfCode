using System.Text.RegularExpressions;

var rows = File.ReadAllLines("Input.txt");

Console.WriteLine(rows.Count(Nice1));
Console.WriteLine(rows.Count(Nice2));
Console.Read();

static bool Nice1(string row)
{
    var naughtyWords = new[] { "ab", "cd", "pq", "xy" };
    if (naughtyWords.Any(row.Contains))
    {
        return false;
    }
    const string vowelChars = "aeiou";
    var vowels = 0;
    var repeat = false;
    for (var i = 0; i < row.Length; i++)
    {
        if (vowelChars.Contains(row[i]))
        {
            vowels++;
        }
        if (i > 0 && row[i] == row[i - 1])
        {
            repeat = true;
        }
    }

    return vowels > 2 && repeat;
}

static bool Nice2(string row)
{
    return Regex.IsMatch(row, @"(..).*?\1") && Regex.IsMatch(row, @"(.).(\1)");
}
