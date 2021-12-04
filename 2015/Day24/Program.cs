long target;
var combos = new List<HashSet<long>>();

Console.WriteLine("Reading data");
var values = File.ReadAllLines("input.txt").Select(long.Parse).ToList();
Console.WriteLine("Calculating target");
target = values.Sum() / 3;
Console.WriteLine($"Target set to {target}");

Console.WriteLine("Generating groups based on target");
CreateGroups(values, target, new List<long>());
Console.WriteLine($"Generated {combos.Count} groups");

Console.WriteLine("Finding smallest group");
var min = combos.Min(c => c.Count);
Console.WriteLine($"Smallest group set to {min}");

Console.WriteLine("Calculating entanglement");
var qe = combos.Where(c => c.Count == min).Min(Multiply);

Console.WriteLine($"Part 1: {qe}");

Console.WriteLine("Calculating target");
target = values.Sum() / 4;
Console.WriteLine($"Target set to {target}");

combos.Clear();

Console.WriteLine("Generating groups based on target");
CreateGroups(values, target, new List<long>());
Console.WriteLine($"Generated {combos.Count} groups");

Console.WriteLine("Finding smallest group");
min = combos.Min(c => c.Count);
Console.WriteLine($"Smallest group set to {min}");

Console.WriteLine("Calculating entanglement");
qe = combos.Where(c => c.Count == min).Min(Multiply);

Console.WriteLine($"Part 2: {qe}");

long Multiply(HashSet<long> values)
{
    var res = values.Aggregate(1L, (acc, cur) => acc * cur);
    return res;
}

void CreateGroups(List<long> numbers, long sum, List<long> combo)
{
    if (sum == 0)
    {
        combos.Add(new HashSet<long>(combo));
        return;
    }
    if (sum < 0)
    {
        return;
    }

    foreach (var number in numbers)
    {
        CreateGroups(numbers.Where(n => n > number).ToList(), sum - number, combo.Append(number).ToList());
    }
}