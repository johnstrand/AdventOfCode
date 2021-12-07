var values = File.ReadAllText("input.txt").Split(',').Select(int.Parse).OrderBy(x => x).ToList();

var candidates = values.Count % 2 == 0 ? new[] { values[values.Count / 2 - 1], values[values.Count / 2] }.Distinct() : new[] { values[values.Count / 2] };

foreach (var candidate in candidates)
{
    var sum = 0;
    foreach (var v in values)
    {
        sum += Math.Abs(candidate - v);
    }
    Console.WriteLine($"Part 1: {sum}");
}

var part2 = int.MaxValue;
for (var candidate = values.First(); candidate <= values[^1]; candidate++)
{
    // Et tu, Bruteforce?
    var sum = 0;
    foreach (var v in values)
    {
        var dist = Math.Abs(candidate - v);
        sum += (dist * (dist + 1) / 2);
    }
    if (sum < part2)
    {
        part2 = sum;
    }
}
Console.WriteLine($"Part 2: {part2}");
