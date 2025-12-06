using AoC.Common;

var input = Input.ReadActual();
var values = input[0..^1].Select(v => v.SplitRemoveEmpty());

var operations = input[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

var part1 = 0L;

for (var i = 0; i < operations.Length; i++)
{
    var operands = values.Select(v => long.Parse(v[i]));

    var result1 = operations[i] switch
    {
        "+" => operands.Sum(),
        _ => operands.Multiply()
    };

    part1 += result1;
}

Console.WriteLine($"Part 1 = {part1}");
