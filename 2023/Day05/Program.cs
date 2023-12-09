using AoC.Common;

var seeds = new List<long>();

var maps = new Dictionary<string, HashSet<Range>>
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

var reverseSequence = new[]
{
    "humidity",
    "temperature",
    "light",
    "water",
    "fertilizer",
    "soil",
    "seed",
};

var currentMap = "";

long MapValue(string map, long value)
{
    var ranges = maps[map];

    var range = ranges.FirstOrDefault(r => r.Source <= value && value < r.Source + r.Length);

    if (range != null)
    {
        var offset = value - range.Source;
        return range.Destination + offset;
    }

    return value;
}

long ReverseMapValue(string map, long value)
{
    var ranges = maps![map];

    var range = ranges.FirstOrDefault(r => r.Destination <= value && value < r.Destination + r.Length);

    if (range != null)
    {
        var offset = value - range.Destination;
        return range.Source + offset;
    }

    return value;
}

long ReverseMapSequence(long value)
{
    foreach (var map in reverseSequence!)
    {
        value = ReverseMapValue(map, value);
    }

    return value;
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
        seeds.AddRange(line.Split(':')[1].ToNumbers64().ToList());
        continue;
    }

    if (line.EndsWith("map:"))
    {
        currentMap = line.Split('-')[0];
        Console.WriteLine($"Reading {currentMap}...");
        continue;
    }

    var numbers = line.ToNumbers64().ToList();
    var destination = numbers[0];
    var source = numbers[1];
    var length = numbers[2];

    maps[currentMap].Add(new Range(source, destination, length));
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

var seedRanges = Enumerable.Range(0, seeds.Count / 2).Select(i => (from: seeds[i * 2], to: seeds[i * 2] + seeds[(i * 2) + 1] - 1)).ToList();

var i = 0;
while (true)
{
    var value = ReverseMapSequence(++i);
    if (seedRanges.Any(r => r.from <= value && value <= r.to))
    {
        Render.Result("Part 2:", i);
        break;
    }
}

internal record Range(long Source, long Destination, long Length);