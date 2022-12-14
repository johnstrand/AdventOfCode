var field = new List<char>();
var w = 0;
foreach (var row in File.ReadAllLines("input.txt"))
{
    w = row.Length;
    field.AddRange(row);
}
var h = field.Count / w;

bool IsValid(int x, int y) => x >= 0 && y >= 0 && x < w && y < h;

bool CanMove(int xFrom, int yFrom, int xTo, int yTo)
{
    if (!IsValid(xTo, yTo))
    {
        return false;
    }

    var cFrom = field![Index(xFrom, yFrom)];
    if (cFrom == 'S')
    {
        cFrom = 'a';
    }

    var cTo = field![Index(xTo, yTo)];

    if (cTo == 'E')
    {
        cTo = 'z';
    }

    return cFrom >= cTo || (cTo - cFrom) == 1;
}

int Index(int x, int y) => (y * w) + x;

int Distance(int xFrom, int yFrom, int xTo, int yTo) => Math.Abs(xFrom - xTo) + Math.Abs(yFrom - yTo);

var startIndex = field.IndexOf('S');
var x = startIndex % w;
var y = startIndex / w;

var deltas = new[]
{
    (x: 1, y: 0),
    (x: -1, y: 0),
    (x: 0, y: 1),
    (x:0,y: -1),
};

var routeCache = new Dictionary<(int x, int y), int>();
var distCache = new Dictionary<(int x, int y), int>();

var minPath = int.MaxValue;

int Traverse(int x, int y, HashSet<(int x, int y)> history)
{
    if (distCache.TryGetValue((x, y), out var cached) && history.Count > cached)
    {
        //Console.WriteLine($"Shortest path (cache): {cached}");
        return -1;
        //return cached;
    }

    if (routeCache.TryGetValue((x, y), out var prev) && prev <= history.Count)
    {
        return -1;
    }

    routeCache[(x, y)] = history.Count;

    //Console.SetCursorPosition(x, y);
    //Console.Write('#');
    if (field[Index(x, y)] == 'E')
    {
        if (history.Count < minPath)
        {
            minPath = history.Count;
            Console.WriteLine($"Shortest path: {minPath}");
        }

        return history.Count;
    }

    if (history.Count >= minPath)
    {
        return -1;
    }

    history.Add((x, y));
    var steps = deltas.Select(o => (x: x + o.x, y: y + o.y)).OrderBy(s => Distance(x, y, s.x, s.y)).ToList();

    foreach (var step in steps)
    {
        if (!CanMove(x, y, step.x, step.y) || history.Contains(step))
        {
            continue;
        }

        var dist = Traverse(step.x, step.y, new(history));
        if (dist == -1)
        {
            continue;
        }

        if (!distCache.ContainsKey((x, y)) || distCache[(x, y)] > dist)
        {
            distCache[(x, y)] = dist;
        }
    }

    return distCache.TryGetValue((x, y), out var c) ? c : -1;
}

/*
Console.WriteLine("Press any key");
Console.ReadKey(true);
*/
Traverse(x, y, new());

Console.WriteLine($"Part 1: {minPath}");

var endIndex = field.IndexOf('E');
var ex = endIndex % w;
var ey = endIndex / w;

var starts = field.Select((c, i) => c == 'a' || c == 'S' ? i : -1).Where(i => i >= 0).OrderBy(i =>
{
    var sx = i % w;
    var sy = i / w;
    return Distance(sx, sy, ex, ey);
}).ToList();

//minPath = int.MaxValue;
var i = 0;

// TODO: Shaaaaaaaaaaaaaaaaaaaaaame
foreach (var start in starts)
{
    i++;
    routeCache.Clear();
    var tx = start % w;
    var ty = start / w;
    var dist = Traverse(tx, ty, new());
    Console.Write($"{i * 1000 / starts.Count} %\r");

    if (dist == -1)
    {
        continue;
    }

    minPath = Math.Min(minPath, dist);
}

Console.WriteLine($"Part 2: {minPath}");
