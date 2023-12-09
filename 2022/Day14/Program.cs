
var lines = new List<Line>();
foreach (var line in File.ReadLines("input.txt"))
{
    var segments = line.Split(" -> ").Select(seg => seg.Split(',').Select(int.Parse).ToArray()).ToArray();
    for (var i = 0; i < segments.Length - 1; i++)
    {
        lines.Add(new()
        {
            X1 = segments[i][0],
            Y1 = segments[i][1],
            X2 = segments[i + 1][0],
            Y2 = segments[i + 1][1],
        });
    }
}

var minX = lines.Min(l => Math.Min(l.X1, l.X2)) - 10;
var maxX = lines.Max(l => Math.Max(l.X1, l.X2)) + 10;

var h = lines.Max(l => Math.Max(l.Y1, l.Y2));
var w = maxX;
static int Norm(int v) => v == 0 ? 0 : v / Math.Abs(v);

foreach (var part1 in new[] { true, false })
{
    var board = new Board(w + 20, h + 5);

    foreach (var line in lines)
    {
        var x = line.X1;
        var y = line.Y1;
        var dx = Norm(line.X2 - line.X1);
        var dy = Norm(line.Y2 - line.Y1);
        board.Set(x, y, '#');
        while (x != line.X2 || y != line.Y2)
        {
            x += dx;
            y += dy;
            board.Set(x, y, '#');
        }
    }

    var done = false;
    while (!done)
    {
        var s = new Sand { X = 500, Y = 0 };

        board.Set(s.X, s.Y, '+');

        while (true)
        {
            if (s.Y > h && part1) // If we've passed max height and we're going by part 1 rules, exit here
            {
                done = true;
                break;
            }

            if (s.Y == h + 1) // If we are at max Y + 2 (h is max Y + 1), it means that we're going by part 2 rules and should place some sand here
            {
                board.Set(s.X, s.Y, 'o');
                break;
            }

            // We're about to do something, so let's clear the old position
            board.Set(s.X, s.Y, ' ');

            if (board.IsEmpty(s.X, s.Y + 1)) // Down is clear?
            {
                s.Y++;
            }
            else if (board.IsEmpty(s.X - 1, s.Y + 1)) // Down-left is clear?
            {
                s.Y++;
                s.X--;
            }
            else if (board.IsEmpty(s.X + 1, s.Y + 1)) // Down-right is clear?
            {
                s.Y++;
                s.X++;
            }
            else // Give up
            {
                board.Set(s.X, s.Y, 'o');
                if (s.Y == 0) // If we've just placed sand on top of the emitter (i.e., part 2), exit here
                {
                    done = true;
                }
                break;
            }
            // If we've made it all the way here, set the new position
            board.Set(s.X, s.Y, '+');
        }
    }
    Console.WriteLine($"Part {(part1 ? 1 : 2)}: {board.Count('o')}");
}

internal class Sand
{
    public int X { get; set; }
    public int Y { get; set; }
}

internal struct Line
{
    public int X1;
    public int Y1;
    public int X2;
    public int Y2;
}

internal class Board(int w, int h)
{
    private readonly List<char> _data = Enumerable.Repeat(' ', w * h).ToList();
    private readonly int _w = w;

    public int Index(int x, int y) => (y * _w) + x;

    public char Get(int x, int y)
    {
        return _data[Index(x, y)];
    }

    public char Set(int x, int y, char v)
    {
        return _data[Index(x, y)] = v;
    }

    public bool IsEmpty(int x, int y)
    {
        return Get(x, y) == ' ';
    }

    public int Count(char c) => _data.Count(d => d == c);
}