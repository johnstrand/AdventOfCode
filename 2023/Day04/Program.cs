var part1 = 0;
var copies = new Dictionary<int, int>();

var index = 1;
foreach (var line in File.ReadAllLines("input.txt"))
{
    var parts = line.Split('|');
    var winners = parts[0].Split(new[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);

    var winningNumbers = new HashSet<int>(winners[2..].Select(int.Parse));

    var own = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
    var matching = own.Where(winningNumbers.Contains).ToList();
    copies[index] = matching.Count;
    var value = matching.Aggregate(0, (acc, _) => acc == 0 ? 1 : acc * 2);
    part1 += value;
    index++;
}

int ResolveCount(int index)
{
    var count = copies![index];
    return 1 + Enumerable.Range(index + 1, count).Sum(ResolveCount);
}

Console.WriteLine(part1);

var part2 = 0;

foreach (var key in copies.Keys)
{
    part2 += ResolveCount(key);
}

Console.WriteLine(part2);
