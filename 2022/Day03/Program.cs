var sum1 = 0;
var sum2 = 0;
var group = new List<string>();

static int getPrio(char c) => char.IsLower(c) ? c - 'a' + 1 : c - 'A' + 27;

foreach (var line in File.ReadAllLines("input.txt"))
{
    group.Add(line);
    if (group.Count == 3)
    {
        var commonGroup = group[0].Where(group[1].Contains).Where(group[2].Contains).Distinct().Single();
        sum2 += getPrio(commonGroup);
        group.Clear();
    }

    var left = line[..(line.Length / 2)];
    var right = line[(line.Length / 2)..];

    var common = left.Where(right.Contains).Distinct().Single();

    sum1 += getPrio(common);
}

Console.WriteLine($"Part 1: {sum1}");
Console.WriteLine($"Part 2: {sum2}");