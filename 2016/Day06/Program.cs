var sorted = File.ReadAllLines("input.txt")
    .SelectMany(row =>
        row.Select((c, i) => new { index = i, value = c }))
    .GroupBy(
        g => g.index,
        g => g.value)
    .Select(g => new
    {
        g.Key,
        Values = g.GroupBy(x => x)
            .Select(x => new
            {
                x.Key,
                Count = x.Count()
            })
    })
    .OrderBy(g => g.Key)
    .Select(g =>
        g.Values
            .OrderByDescending(v => v.Count)
            .Select(v => v.Key)
            .ToList())
    .ToList();

var part1 = string.Concat(sorted.Select(r => r[0]));
var part2 = string.Concat(sorted.Select(r => r[^1]));

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");
