using AoC.Common;

var list1 = new List<int>();
var list2 = new List<int>();

foreach (var line in Input.ReadActual())
{
    var (v1, v2) = line
        .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .SelectOne(s => (int.Parse(s[0]), int.Parse(s[1])));

    list1.Add(v1);
    list2.Add(v2);
}

var part1 = list1
    .Order()
    .Zip(list2.Order())
    .Select(t => Math.Abs(t.First - t.Second))
    .Sum();

Console.WriteLine($"Part 1: {part1}");

var part2 = list1
    .Select(v => v * list2.Count(v2 => v2 == v))
    .Sum();

Console.WriteLine($"Part 2: {part2}");
