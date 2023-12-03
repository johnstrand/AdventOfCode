using System.Numerics;

var part1 = 0;
var part2 = 0;
var y = 0;

var numbers = new List<Number>();
var blocks = new List<Block>();

foreach (var line in File.ReadLines("input.txt"))
{
    for (var x = 0; x < line.Length; x++)
    {
        if (line[x] == '.')
        {
            continue;
        }
        if (char.IsDigit(line[x]))
        {
            var positions = new List<Vector2>
            {
                new(x,y)
            };

            var number = line[x] - '0';
            while (x + 1 < line.Length && char.IsDigit(line[x + 1]))
            {
                number = (number * 10) + (line[x + 1] - '0');
                x++;
                positions.Add(new(x, y));
            }

            numbers.Add(new Number(number, [.. positions]));
        }
        else
        {
            blocks.Add(new(line[x], new(x, y)));
        }
    }
    y++;
}

foreach (var number in numbers)
{
    if (!number.Positions.Any(p => blocks.Any(b => Vector2.Distance(p, b.Position) < 2)))
    {
        continue;
    }
    part1 += number.Value;
}

foreach (var block in blocks.Where(b => b.Value == '*'))
{
    var adjacentNumbers = numbers.Where(n => n.Positions.Any(p => Vector2.Distance(p, block.Position) < 2)).ToList();
    if (adjacentNumbers.Count != 2)
    {
        continue;
    }
    part2 += adjacentNumbers[0].Value * adjacentNumbers[1].Value;
}

Console.WriteLine(part1);
Console.WriteLine(part2);

internal record Number(int Value, params Vector2[] Positions);
internal record Block(char Value, Vector2 Position);
