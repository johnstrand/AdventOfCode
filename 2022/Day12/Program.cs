using System.Diagnostics.CodeAnalysis;

using AoC.Common;

var maze = new Maze(File.ReadAllLines("input.txt"));

var path = maze.Solve();

Console.WriteLine($"Part 1: {path.Count - 1}");

var startPoints = maze.Find(c => c == 'a').ToList();
var index = 0;
var path2 = path.Count - 1;

foreach (var start in startPoints)
{
    index++;
    path = maze.Solve(start, maze.End);
    Console.Write($"Part 2: {path2} ({index:N0} / {startPoints.Count:N0} :: {index * 100 / startPoints.Count} %)\r");
    if (path.Count == 0)
    {
        continue;
    }
    path2 = Math.Min(path.Count + 1, path2);
}
Console.WriteLine();

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

int Index(int x, int y) => y * w + x;

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

class Maze
{
    private readonly int _w;
    private readonly int _h;
    private readonly List<char> _field = new();
    private readonly Dictionary<Point, List<Point>> _edges = new();

    public Point Start { get; } = new(-1, -1);
    public Point End { get; } = new(-1, -1);

    public Maze(IEnumerable<string> content)
    {
        foreach (var row in content)
        {
            var mutableRow = row;

            _w = mutableRow.Length;

            var startIndex = mutableRow.IndexOf('S');
            if (startIndex > -1)
            {
                Start = new(startIndex, _h);
                mutableRow = mutableRow.Replace('S', 'a');
            }

            var endIndex = mutableRow.IndexOf('E');
            if (endIndex > -1)
            {
                End = new(endIndex, _h);
                mutableRow = mutableRow.Replace('E', 'z');
            }

            _h++;

            _field.AddRange(mutableRow);
        }

        for (var y = 0; y < _h; y++)
        {
            for (var x = 0; x < _w; x++)
            {
                _edges[new(x, y)] = new();
                for (var ty = Math.Max(y - 1, 0); ty <= Math.Min(y + 1, _h - 1); ty++)
                {
                    for (var tx = Math.Max(x - 1, 0); tx <= Math.Min(x + 1, _w - 1); tx++)
                    {
                        if (tx == x && ty == y)
                        {
                            continue;
                        }

                        if (tx != x && ty != y)
                        {
                            continue;
                        }

                        var fromIndex = y * _w + x;
                        var toIndex = ty * _w + tx;
                        var fromVal = _field[fromIndex];
                        var toVal = _field[toIndex];

                        if (toVal - fromVal <= 1)
                        {
                            _edges[new(x, y)].Add(new(tx, ty));
                        }
                    }
                }
            }
        }
    }

    public IEnumerable<Point> Find(Func<char, bool> predicate)
    {
        for (var index = 0; index < _field.Count; index++)
        {
            if (predicate(_field[index]))
            {
                yield return new(
                    index % _w,
                    index / _w
                    );
            }
        }
    }

    public List<Point> Solve()
    {
        return Solve(Start, End);
    }

    public List<Point> Solve(Point start, Point end)
    {
        // Shortest distance between the node and the start, initially set to Infinity (or thereabouts)
        var dist = _edges.Keys.ToDictionary(k => k, _ => int.MaxValue);

        // Given a node, tracks the best node to move to reach the end as quickly as possible,
        // initially set to Unknown
        var prev = _edges.Keys.ToDictionary(k => k, _ => (Point?)null);

        // List of all points to process
        var q = _edges.Keys.ToList();

        // Helper HashSet for better performance in lookup
        var inQ = new HashSet<Point>(q);

        dist[start] = 0;

        // Ensure that the start point is the first item in the queue
        q.Remove(start);
        q.Insert(0, start);
        var insertionIndex = 0; // Tracked index to ensure that we insert nodes at the appropriate places in the queue

        // While we have nodes to process
        while (q.Count > 0)
        {
            var next = q.First(); // Grab the first node on the list

            // Already at the end, exit loop
            if (next == end)
            {
                break;
            }

            q.RemoveAt(0); // Remove 'next' from the queue
            inQ.Remove(next);

            // Fetch all nearby nodes that are still in the queue
            var neighbors = _edges[next].Where(inQ.Contains).ToList();

            // Calculate distance travelled to reach the node
            var distance = dist[next] + 1;

            foreach (var n in neighbors)
            {
                // If we've travelled a shorter distance than before
                if (distance < dist[n])
                {
                    // Move the node as close to the front as insertionIndex allows us
                    // This ensures that it is processed before other, further away nodes,
                    // but not before closer nodes
                    q.Remove(n);
                    q.Insert(insertionIndex++, n);

                    // Record the new, shorter, distance
                    dist[n] = distance;

                    // Mark next -> as the desirable step to take
                    prev[n] = next;
                }
            }

            // Ensure that the index doesn't grow uncontrollably
            insertionIndex = Math.Max(0, insertionIndex - 1);
        }

        // Time to back-track and solve generate the path
        var path = new List<Point>();
        var current = (Point?)end;

        while (current != null)
        {
            path.Insert(0, current);
            current = prev[current];
        }

        // If we managed to find our way back to the start,
        // return the list of nodes, otherwise return an empty list
        return path[0] == start ? path : new();
    }
}


