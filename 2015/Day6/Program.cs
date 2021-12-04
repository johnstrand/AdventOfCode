using System.Drawing;
using System.Text.RegularExpressions;

Part1();
Part2();
Console.Read();

static void Part1()
{
    var grid = new Grid(1000, 1000);
    grid.Register("turn on", (state, start, end) =>
    {
        For(start, end, p => state[p.X, p.Y] = 1);

        return state;
    });

    grid.Register("turn off", (state, start, end) =>
    {
        For(start, end, p => state[p.X, p.Y] = 0);

        return state;
    });

    grid.Register("toggle", (state, start, end) =>
    {
        For(start, end, p => state[p.X, p.Y] = (state[p.X, p.Y] == 0 ? 1 : 0));

        return state;
    });
    var frame = 1;
    foreach (var line in File.ReadAllLines("Input.txt"))
    {
        Console.Write($"Processing frame {frame:00000}\r");
        var m = Regex.Match(line, "^(?<command>.+) (?<from>.+?) through (?<to>.+?)$");
        var cmd = m.Groups["command"].Value;
        var from = ParsePoint(m.Groups["from"].Value);
        var to = ParsePoint(m.Groups["to"].Value);
        grid.Raise(cmd, from, to);
        frame++;
    }
    Console.WriteLine();
    Console.WriteLine(grid.Count(b => b > 0));
}

static void Part2()
{
    var grid = new Grid(1000, 1000);
    grid.Register("turn on", (state, start, end) =>
    {
        For(start, end, p => state[p.X, p.Y]++);

        return state;
    });

    grid.Register("turn off", (state, start, end) =>
    {
        For(start, end, p => state[p.X, p.Y] = Math.Max(state[p.X, p.Y] - 1, 0));

        return state;
    });

    grid.Register("toggle", (state, start, end) =>
    {
        For(start, end, p => state[p.X, p.Y] += 2);

        return state;
    });
    var frame = 1;
    foreach (var line in File.ReadAllLines("Input.txt"))
    {
        Console.Write($"Processing frame {frame:00000}\r");
        var m = Regex.Match(line, "^(?<command>.+) (?<from>.+?) through (?<to>.+?)$");
        var cmd = m.Groups["command"].Value;
        var from = ParsePoint(m.Groups["from"].Value);
        var to = ParsePoint(m.Groups["to"].Value);
        grid.Raise(cmd, from, to);
        //grid.AsBitmap().Save($"output1\\frame{frame.ToString("00000")}.png", ImageFormat.Png);
        frame++;
    }
    Console.WriteLine();
    Console.WriteLine(grid.Sum());
}

static Point ParsePoint(string input)
{
    var parts = input.Split(',').Select(int.Parse).ToList();
    return new Point(parts[0], parts[1]);
}

static void For(Point start, Point end, Action<Point> action)
{
    for (var y = start.Y; y <= end.Y; y++)
    {
        for (var x = start.X; x <= end.X; x++)
        {
            action(new Point(x, y));
        }
    }
}

internal class Grid
{
    private int[,] grid;
    private readonly int width;
    private readonly int height;
    private readonly Dictionary<string, Func<int[,], Point, Point, int[,]>> reducers = new();
    public int Sum()
    {
        var sum = 0;
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                sum += grid[x, y];
            }
        }
        return sum;
    }
    public int Count(Func<int, bool> counter)
    {
        var count = 0;
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                count += counter(grid[x, y]) ? 1 : 0;
            }
        }
        return count;
    }
    public Grid(int width, int height)
    {
        this.width = width;
        this.height = height;
        grid = new int[width, height];
    }

    public void Register(string ev, Func<int[,], Point, Point, int[,]> reducer)
    {
        reducers.Add(ev, reducer);
    }

    public void Raise(string ev, Point start, Point end)
    {
        grid = reducers[ev](grid, start, end);
    }

    public Image AsBitmap()
    {
        var img = new Bitmap(width, height);
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                img.SetPixel(x, y, grid[x, y] > 0 ? Color.White : Color.Black);
            }
        }
        return img;
    }
}
