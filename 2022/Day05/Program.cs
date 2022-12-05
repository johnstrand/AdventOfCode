using System.Text.RegularExpressions;

var content = File.ReadAllLines("input.txt").ToList();
var answers = new List<string>();

foreach (var isPart1 in new[] { true, false })
{
    var numberRow = content.FindIndex(c => c.Length > 2 && char.IsNumber(c[1]));

    var stacks = new List<Stack<char>>();

    void Transfer(int count, int from, int to)
    {
        var transferStack = new Stack<char>();
        while (count-- > 0)
        {
            transferStack.Push(stacks[from].Pop());
            if (isPart1)
            {
                stacks[to].Push(transferStack.Pop());
            }
        }
        while (transferStack.Count > 0)
        {
            stacks[to].Push(transferStack.Pop());
        }
    }

    for (var i = 0; i < content[numberRow].Length; i++)
    {
        if (!char.IsNumber(content[numberRow][i]))
        {
            continue;
        }
        stacks.Add(new());
        var ix = numberRow - 1;
        while (ix >= 0 && char.IsLetter(content[ix][i]))
        {
            stacks[^1].Push(content[ix--][i]);
        }
    }

    for (var rowNo = numberRow + 2; rowNo < content.Count; rowNo++)
    {
        var instr = content[rowNo];
        var match = InstructionMatcher().Match(instr);
        var count = int.Parse(match.Groups[1].Value);
        var from = int.Parse(match.Groups[2].Value) - 1;
        var to = int.Parse(match.Groups[3].Value) - 1;

        Transfer(count, from, to);

        Console.Clear();

        for (var i = 0; i < stacks.Count; i++)
        {
            Console.WriteLine($"{i + 1}: {string.Join(" | ", stacks[i].Reverse())}");
        }

        Console.WriteLine($"{rowNo + 1:N0} / {content.Count:N0}");

        await Task.Delay(10);
    }

    answers.Add($"Part {(isPart1 ? 1 : 2)}: {string.Concat(stacks.Select(s => s.Peek()))}");
}

foreach (var a in answers)
{
    Console.WriteLine(a);
}

internal partial class Program
{
    [GeneratedRegex("move (\\d+) from (\\d+) to (\\d+)")]
    private static partial Regex InstructionMatcher();
}