var part1 = 0;
var part2 = 0;
foreach (var pair in File.ReadAllLines("input.txt"))
{
    var ranges = pair
        .Split(',')
        .SelectMany(section => section
            .Split('-')
        )
        .Select(int.Parse)
        .ToList();

    var (s1, e1, s2, e2) = (ranges[0], ranges[1], ranges[2], ranges[3]);

    if ((s1 <= s2 && e1 >= e2) || (s2 <= s1 && e2 >= e1))
    {
        part1++;
    }

    if ((s1 >= s2 && s1 <= e2) || (s2 >= s1 && s2 <= e1))
    {
        part2++;
    }
}

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");