using AoC.Common;

var seeds = new List<long>();

var maps = new Dictionary<string, Dictionary<long, long>>
{
    ["seed"] = [],
    ["soil"] = [],
    ["fertilizer"] = [],
    ["water"] = [],
    ["light"] = [],
    ["temperature"] = [],
    ["humidity"] = [],
};

var sequence = new[]
{
    "seed",
    "soil",
    "fertilizer",
    "water",
    "light",
    "temperature",
    "humidity",
};

var currentMap = "";

long MapValue(string map, long value)
{
    return maps![map].TryGetValue(value, out var result) ? result : value;
}

Console.WriteLine("Reading input...");
foreach (var line in File.ReadAllLines("input.txt"))
{
    if (string.IsNullOrWhiteSpace(line))
    {
        currentMap = "";
        continue;
    }

    if (line.StartsWith("seeds:"))
    {
        seeds.AddRange(line.Split(':')[1].ToNumbers().ToList());
        continue;
    }

    if (line.EndsWith("map:"))
    {
        currentMap = line.Split('-')[0];
        Console.WriteLine($"Reading {currentMap}...");
        continue;
    }

    var numbers = line.ToNumbers().ToList();
    var destination = numbers[1];
    var source = numbers[0];
    var length = numbers[2];

    for (var i = 0; i < length; i++)
    {
        maps[currentMap][destination + i] = source + i;
    }
}

var min = long.MaxValue;
foreach (var seed in seeds)
{
    var value = seed;
    foreach (var step in sequence)
    {
        value = MapValue(step, value);
    }
    if (value < min)
    {
        min = value;
    }
}

Render.Result("Part 1", min);