using AoC.Common;

var input = Input.ReadActual();
var values = input[0..^1].Select(v => v.ToNumbers64().ToList()).ToList();

var operations = input[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

var part1 = 0L;

for (var i = 0; i < operations.Length; i++)
{
    var operands = values.Select(v => v[i]);

    var result = operations[i] switch
    {
        "+" => operands.Sum(),
        _ => operands.Multiply()
    };

    part1 += result;
}

Console.WriteLine($"Part 1 = {part1}");

var inputs = input[0..^1];

var numberGroups = new List<List<long>> { new() };

for (var i = 0; i < inputs[0].Length; i++)
{
    if (inputs.All(p => p[i] == ' '))
    {
        numberGroups.Add([]);
        continue;
    }

    var number = long.Parse(string.Concat(inputs.Select(p => p[i])));

    numberGroups[^1].Add(number);
}

var part2 = 0L;

for (var i = 0; i < operations.Length; i++)
{
    var operands = numberGroups[i];

    var result = operations[i] switch
    {
        "+" => operands.Sum(),
        _ => operands.Multiply()
    };

    part2 += result;
}

Console.WriteLine($"Part 2 = {part2}");
