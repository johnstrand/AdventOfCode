var calorieCounts = File.ReadAllLines("input.txt").Aggregate(new List<int> { 0 }, (acc, cur) =>
{
    if (string.IsNullOrWhiteSpace(cur))
    {
        acc.Add(0);
        return acc;
    }

    acc[^1] += int.Parse(cur);

    return acc;
});

var max = calorieCounts.Max();

Console.WriteLine($"Part 1: {max}");

var top3 = calorieCounts.OrderByDescending(c => c).Take(3).Sum();

Console.WriteLine($"Part 2: {top3}");