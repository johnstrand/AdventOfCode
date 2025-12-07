using AoC.Common;

Part1();

static void Part1()
{
    var grid = new Grid<char>(Input.ReadTest(), c => c);

    var (x, y) = grid.GetMatching(c => c == 'S').First();
    var part1 = 0;

    var toProcess = new Stack<(int x, int y)>();

    toProcess.Push((x, y + 1));

    while (toProcess.TryPop(out var next))
    {
        if (grid[next.x, next.y] == '^')
        {
            var incr = false;

            if (grid[next.x + 1, next.y] == '.')
            {
                toProcess.Push((next.x + 1, next.y));
                incr = true;
            }

            if (grid[next.x - 1, next.y] == '.')
            {
                toProcess.Push((next.x - 1, next.y));
                incr = true;
            }

            if (incr)
            {
                part1++;
            }

            continue;
        }

        grid[next.x, next.y] = '|';

        if (next.y == grid.Height - 1)
        {
            continue;
        }

        toProcess.Push((next.x, next.y + 1));
    }

    Console.WriteLine($"Part 1 = {part1}");
}

static void Part2()
{
    var grid = new Grid<char>(Input.ReadTest(), c => c);

    var p = grid.GetMatching(c => c == 'S').First().ToPoint();

    var nodes = new Dictionary<Point, HashSet<Point>>();

    var toProcess = new Stack<Point>();
    toProcess.Push(p);

    bool Link(Point a, Point b)
    {
        nodes.TryAdd(b, []);
        if (nodes.TryGetValue(a, out var following))
        {
            return following.Add(b);
        }

        nodes[a] = [b];
        return true;
    }

    while (toProcess.TryPop(out p))
    {
        if (p.Y == grid.Height)
        {
            continue;
        }

        if (grid[p] == '^')
        {
            var left = p.Offset(-1, 0);

            if (Link(p, left))
            {
                toProcess.Push(left);
            }

            var right = p.Offset(1, 0);

            if (Link(p, right))
            {
                toProcess.Push(right);
            }

            continue;
        }

        var next = p.Offset(0, 1);

        if (Link(p, next))
        {
            toProcess.Push(next);
        }
    }

    var endnodes = nodes.Where(n => n.Value.Count == 0).Select(n => n.Key).ToList();

    var endCount = 0;
    foreach (var n in endnodes)
    {
        toProcess.Push(n);
        endCount++;

        while (toProcess.TryPop(out var t))
        {
            var next = nodes.Where(n => n.Value.Contains(t)).Select(n => n.Key).ToList();
            foreach (var nt in next)
            {
                toProcess.Push(nt);
            }
            Console.Write($"\rPending={toProcess.Count:n0}. Progress={endCount}/{endnodes.Count}");
        }
    }

    var part2 = 0;
    void Process(Point p, List<string> route)
    {
        if (!nodes.TryGetValue(p, out var following))
        {
            part2++;
            Console.Write($"\rPart 2 = {part2}");
            return;
        }

        foreach (var next in following)
        {
            Process(next, [.. route, next.ToString()]);
        }
    }


    p = grid.GetMatching(c => c == 'S').First().ToPoint();

    Process(p, []);

    Console.WriteLine($"\rPart 2 = {part2}");

#if false
    var paths = new List<List<Point>>();

    var toProcess = new Stack<(Point pt, List<Point> path)>();
    toProcess.Push((p, []));

    void Add(List<Point> points)
    {
        if (paths.Exists(p => p.SequenceEqual(points)))
        {
            return;
        }

        paths.Add(points);

        Console.Write($"\rCache size: {paths.Count:N0}");
    }

    while (toProcess.TryPop(out var next))
    {
        if (next.pt.Y == grid.Height)
        {
            Add(next.path);
            continue;
        }

        if (grid[next.pt] == '^')
        {
            var left = next.pt.Offset(-1, 0);

            var existing = paths.Where(p => p.Contains(left)).ToList();

            if (existing.Count > 0)
            {
                foreach (var e in existing)
                {
                    var newPath = next.path.Concat(e.Skip(e.IndexOf(left))).ToList();
                    Add(newPath);
                }
            }
            else
            {
                toProcess.Push((left, next.path.ToList()));
            }

            var right = next.pt.Offset(1, 0);

            existing = [.. paths.Where(p => p.Contains(right))];

            if (existing.Count > 0)
            {
                foreach (var e in existing)
                {
                    var newPath = next.path.Concat(e.Skip(e.IndexOf(right))).ToList();
                    Add(newPath);
                }
            }
            else
            {
                toProcess.Push((right, next.path.ToList()));
            }

            continue;
        }

        toProcess.Push((next.pt.Offset(0, 1), next.path.With(next.pt)));
    }

    Console.WriteLine();
    Console.WriteLine($"Part 2 = {paths.Select(p => string.Join(", ", p)).Distinct().Count()}");
#endif
}

Part2();

