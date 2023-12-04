var part1 = 0;

foreach (var line in File.ReadAllLines("input.txt"))
{
    var parts = line.Split('|');
    var winners = parts[0].Split(new[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);

    var winningNumbers = new HashSet<int>(winners[2..].Select(int.Parse));

    var own = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
    var value = own.Where(winningNumbers.Contains).Aggregate(0, (acc, _) => acc == 0 ? 1 : acc * 2);
    Console.WriteLine($"{line} => {value}");
    part1 += value;
}

Console.WriteLine(part1);
