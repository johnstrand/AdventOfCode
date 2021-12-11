var w = 0;
var h = 0;
var grid = new List<int>();

foreach (var line in File.ReadAllLines("input.txt"))
{
    h++;
    w = line.Length;
    grid.AddRange(line.Select(c => c - '0'));
}

bool isValid(int x, int y)
{
    return x >= 0 && y >= 0 && x < w && y < h;
}

int getValue(int x, int y)
{
    return grid[(y * w) + x];
}

var deltas = new[]
{
    (x: 1, y: 0),
    (x: -1, y: 0),
    (x: 0, y: 1),
    (x: 0, y: -1),
};

int getBasin(int x, int y)
{
    var seen = new HashSet<(int x, int y)>();
    var toProcess = new Queue<(int x, int y)>(new[] { (x, y) });
    while (toProcess.Count > 0)
    {
        var next = toProcess.Dequeue();
        if (!seen.Add(next))
        {
            continue;
        }
        foreach (var (dx, dy) in deltas)
        {
            var (px, py) = (next.x + dx, next.y + dy);
            if (!isValid(px, py) || getValue(px, py) == 9)
            {
                continue;
            }
            toProcess.Enqueue((px, py));
        }
    }

    return seen.Count;
}

bool isMinima(int x, int y)
{
    var value = getValue(x, y);
    var adder = Adder(x, y);
    return deltas.Select(adder).Where(d => isValid(d.x, d.y)).All(d => value < getValue(d.x, d.y));
}

Func<(int x, int y), (int x, int y)> Adder(int xb, int yb)
{
    return pos => (xb + pos.x, yb + pos.y);
}

var value = 0;
var basins = new List<int>();
for (var y = 0; y < h; y++)
{
    for (var x = 0; x < w; x++)
    {
        var v = getValue(x, y);
        if (isMinima(x, y))
        {
            value += v + 1;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(v);
            basins.Add(getBasin(x, y));
        }
        else
        {
            var c = v * 255 / 10;
            Console.Write($"\x1b[38;2;{c};{c};{c}m{v}");
        }
    }
    Console.WriteLine();
    await Task.Delay(100);
}

Console.WriteLine();
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine($"Part 1: {value}");
Console.WriteLine($"Part 2: {basins.OrderByDescending(x => x).Take(3).Aggregate((acc, cur) => acc * cur)}");
