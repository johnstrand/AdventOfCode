using AoC.Common;

var part1 = 0;
var copies = new Dictionary<int, int>();

var index = 1;
foreach (var line in File.ReadAllLines("input.txt"))
{
    var (info, numbers) = line.ToTuple('|');
    var winners = info.SplitRemoveEmpty(' ', ':');

    var winningNumbers = winners[2..].ToNumbers32().ToHashSet();

    var own = numbers.ToNumbers32().ToList();
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

Render.Result("Part 1", part1);

var part2 = 0;

foreach (var key in copies.Keys)
{
    part2 += ResolveCount(key);
}

Render.Result("Part 2", part2);
