var limits = new Dictionary<string, int>
{
    ["red"] = 12,
    ["green"] = 13,
    ["blue"] = 14
};

var part1 = 0;
var part2 = 0;
foreach (var row in File.ReadAllLines("input.txt"))
{
    var parts = row.Split(new[] { ',', ';', ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
    var id = int.Parse(parts[1]);
    var value1 = id;

    var minimums = new Dictionary<string, int>
    {
        ["red"] = 0,
        ["green"] = 0,
        ["blue"] = 0
    };

    for (var i = 2; i < parts.Length; i += 2)
    {
        var value = int.Parse(parts[i]);
        var color = parts[i + 1];

        if (value > minimums[color])
        {
            minimums[color] = value;
        }

        if (value > limits[color])
        {
            value1 = 0;
        }
    }

    // Console.WriteLine($"{id}: {string.Join(", ", minimums.Select(kv => $"{kv.Key} = {kv.Value}"))}");

    part1 += value1;
    part2 += minimums.Values.Aggregate(1, (a, b) => a * b);
}

Console.WriteLine(part1);
Console.WriteLine(part2);
