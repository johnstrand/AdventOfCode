var grid = File.ReadAllLines("input.txt").SelectMany(row => row.Select(c => c - '0')).ToList();
var size = (int)Math.Sqrt(grid.Count);

var visible = new HashSet<(int x, int y)>();
int Height(int x, int y) => grid![y * size + x];
bool IsOob(int x, int y) => x < 0 || y < 0 || x >= size || y >= size;
int Incr(int v) => v > 0 ? 1 : v < 0 ? -1 : 0;

int NormalizeV(int v) => v == 0 ? 0 : v / Math.Abs(v);

(int dx, int dy) Normalize(int dx, int dy) => (NormalizeV(dx), NormalizeV(dy));

bool IsVisible(int x, int y)
{
    var h = Height(x, y);

    var offsets = new Queue<(int dx, int dy)>(new[]
    {
        (1, 0),
        (0, 1),
        (-1, 0),
        (0, -1),
    });

    while (offsets.Count > 0)
    {
        var (dx, dy) = offsets.Dequeue();
        if (IsOob(x + dx, y + dy))
        {
            return true;
        }

        if (Height(x + dx, y + dy) >= h)
        {
            continue;
        }

        offsets.Enqueue((dx + Incr(dx), dy + Incr(dy)));
    }

    return false;
}

int VisibleScore(int x, int y)
{
    var h = Height(x, y);

    var offsets = new Queue<(int dx, int dy)>(new[]
    {
        (1, 0),
        (0, 1),
        (-1, 0),
        (0, -1),
    });

    var offsetCounts = offsets.ToDictionary(v => v, _ => 0);

    while (offsets.Count > 0)
    {
        var (dx, dy) = offsets.Dequeue();
        if (IsOob(x + dx, y + dy))
        {
            continue;
        }

        var seenHeight = Height(x + dx, y + dy);

        offsetCounts[Normalize(dx, dy)]++;

        if (seenHeight >= h)
        {
            continue;
        }

        offsets.Enqueue((dx + Incr(dx), dy + Incr(dy)));
    }

    return offsetCounts.Values.Aggregate((acc, cur) => acc * cur);
}

// Edges are always visible
for (var i = 0; i < size; i++)
{
    visible.Add((i, 0));
    visible.Add((i, size - 1));
    visible.Add((0, i));
    visible.Add((size - 1, i));
}

Console.Clear();

for (var y = 0; y < size; y++)
{
    for (var x = 0; x < size; x++)
    {
        if (IsVisible(x, y))
        {
            visible.Add((x, y));
        }
    }
}

Console.WriteLine($"Part 1: {visible.Count}");

var part2 = 0;

for (var y = 0; y < size; y++)
{
    for (var x = 0; x < size; x++)
    {
        part2 = Math.Max(VisibleScore(x, y), part2);
    }
}

Console.WriteLine($"Part 2: {part2}");
