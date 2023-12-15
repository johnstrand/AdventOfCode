using System.Text.RegularExpressions;

using AoC.Common;

(char[] seq, Dictionary<string, (string left, string right)> instr) ReadFile(string name)
{
    var input = File.ReadAllLines(name);
    var seq = input[0].ToArray();
    var instr = new Dictionary<string, (string left, string right)>();

    for (var i = 2; i < input.Length; i++)
    {
        var match = Regex.Match(input[i], @"(\w{3}) = \((\w{3}), (\w{3})\)");
        instr[match.Groups[1].Value] = (match.Groups[2].Value, match.Groups[3].Value);
    }

    return (seq, instr);
}

IEnumerable<(string step, int count)> Traverse(string step, char[] seq, Dictionary<string, (string left, string right)> instr, string end)
{
    var stepCount = 0;

    while (step != end)
    {
        yield return (step, stepCount);
        var index = stepCount % seq.Length;
        step = seq[index] == 'L' ? instr[step].left : instr[step].right;
        stepCount++;
    }
    yield return (step, stepCount);
}

var (seq1, instr1) = ReadFile("input.txt");

var stepCount = Traverse("AAA", seq1, instr1, "ZZZ").Last().count;

Render.Result("Part 1", stepCount);

var (seq2, instr2) = ReadFile("input.txt");

var startPoints = instr2.Keys.Where(k => k[^1] == 'A').ToList();
var cycles = new List<int>();

foreach (var pt in startPoints)
{
    var cycle = Traverse(pt, seq2, instr2, "ZZZ").First(s => s.step[^1] == 'Z').count;
    cycles.Add(cycle);
}

long Gcf(long a, long b)
{
    while (b != 0)
    {
        (a, b) = (b, a % b);
    }

    return a;
}

var val = cycles.Aggregate(1L, (acc, cur) => (acc * cur) / Gcf(acc, cur));

Render.Result("Part 2", val);

Console.WriteLine();
