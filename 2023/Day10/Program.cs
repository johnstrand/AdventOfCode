using AoC.Common;

var w = 0;
var h = 0;

var grid = new List<char>();

int Index(int x, int y) => y * w + x;
(int x, int y) Pos(int index) => (index % w, index / w);

bool Is(int x, int y, params char[] blocks)
{
    return x < 0 || y < 0 || x >= w || y >= h ? false : blocks.Contains(grid![Index(x, y)]);
}

bool ConnectsUp(int x, int y)
{
    return Is(x, y, '|', 'L', 'J', 'S') && Is(x, y - 1, '|', '7', 'F');
}

bool ConnectsDown(int x, int y)
{
    return Is(x, y, '|', '7', 'F', 'S') && Is(x, y + 1, '|', 'L', 'J');
}

bool ConnectsLeft(int x, int y)
{
    return Is(x, y, '-', '7', 'J', 'S') && Is(x - 1, y, '-', 'F', 'L');
}

bool ConnectsRight(int x, int y)
{
    return Is(x, y, '-', 'F', 'L', 'S') && Is(x + 1, y, '-', '7', 'J');
}

foreach (var line in File.ReadAllLines("input-test.txt"))
{
    w = line.Length;
    h++;
    grid.AddRange(line);
}

var distances = Enumerable.Repeat(-1, grid.Count).ToList();

var startIndex = grid.IndexOf('S');
var (x, y) = Pos(startIndex);

var pending = new Queue<(int x, int y, int cost)>();
pending.Enqueue((x, y, 0));

while (pending.TryDequeue(out var item))
{
    var index = Index(item.x, item.y);

    if (distances[index] != -1 && item.cost >= distances[index])
    {
        continue;
    }

    /*
    Console.SetCursorPosition(item.x, item.y);
    Console.Write(item.cost % 10);
    */

    distances[index] = item.cost;

    if (ConnectsUp(item.x, item.y))
    {
        pending.Enqueue((item.x, item.y - 1, item.cost + 1));
    }

    if (ConnectsDown(item.x, item.y))
    {
        pending.Enqueue((item.x, item.y + 1, item.cost + 1));
    }

    if (ConnectsLeft(item.x, item.y))
    {
        pending.Enqueue((item.x - 1, item.y, item.cost + 1));
    }

    if (ConnectsRight(item.x, item.y))
    {
        pending.Enqueue((item.x + 1, item.y, item.cost + 1));
    }
}

var part1 = distances.Max();
Render.Result("Part 1", part1);

int ix;

int CountCrossings(int x, int y, int dx, int dy)
{
    var count = 0;
    while (x >= 0 && y >= 0 && x < w && y < h)
    {
        var ix = Index(x, y);
        if (distances![ix] != -1)
        {
            count++;
            if (dx == -1 && grid[ix] is '7' or 'J')
            {
                x--;
                while (grid[Index(x, y)] == '-')
                {
                    x--;
                }
            }
            else if (dx == 1 && grid[ix] is 'L' or 'F')
            {
                x++;
                while (grid[Index(x, y)] == '-')
                {
                    x++;
                }
            }
            else if (dy == -1 && grid[ix] is 'L' or 'J')
            {
                y--;
                while (grid[Index(x, y)] == '|')
                {
                    y--;
                }
            }
            else if (dy == 1 && grid[ix] is 'F' or 'J')
            {
                y++;
                while (grid[Index(x, y)] == '|')
                {
                    y++;
                }
            }
        }
        x += dx;
        y += dy;
    }

    return count;
}

bool IsOdd(int v) => v == 0 || (v & 1) == 1;

while ((ix = distances.IndexOf(-1)) != -1)
{
    var (sx, yy) = Pos(ix);

    distances[ix] = IsOdd(CountCrossings(x, y, 1, 0)) || IsOdd(CountCrossings(x, y, -1, 0)) || IsOdd(CountCrossings(x, y, 0, 1)) || IsOdd(CountCrossings(x, y, 0, -1))
        ? -3
        : -2;

    /*
    var shouldDelete = false;
    var toDelete = new HashSet<int>();
    var toSearch = new Queue<(int x, int y)>();
    toSearch.Enqueue(Pos(ix));

    while (toSearch.TryDequeue(out var pos))
    {
        if (pos.x < 0 || pos.y < 0 || pos.x >= w || pos.y >= h)
        {
            shouldDelete = true;
            continue;
        }

        ix = Index(pos.x, pos.y);

        if (distances[ix] != -1 || !toDelete.Add(ix))
        {
            continue;
        }

        toSearch.Enqueue((pos.x - 1, pos.y));
        toSearch.Enqueue((pos.x + 1, pos.y));
        toSearch.Enqueue((pos.x, pos.y - 1));
        toSearch.Enqueue((pos.x, pos.y + 1));
    }

    foreach (var deleteIndex in toDelete)
    {
        distances[deleteIndex] = shouldDelete ? -2 : -3;
    }
    */
}

for (var i = 0; i < grid.Count; i += w)
{
    Console.WriteLine(string.Concat(distances.GetRange(i, w).Select(x => x == -2 ? "." : x == -3 ? "?" : "#")));
}

var part2 = distances.Count(d => d == -3);

// Console.SetCursorPosition(0, h + 1);

Render.Result("Part 2", part2);

