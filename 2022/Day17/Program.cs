using AoC.Common;

var templates = new[]
{
    "####",

    " # " +
    "###" +
    " # ",

    "  #" +
    "  #" +
    "###",

    "#" +
    "#" +
    "#" +
    "#",

    "##" +
    "##"
};

var layouts = new[]
{
    (4, 1),
    (3, 3),
    (3, 3),
    (1, 4),
    (2, 2)
};

var pieces = new List<Piece>();

for (var i = 0; i < templates.Length; i++)
{
    var piece = new Piece();

    var (w, h) = layouts[i];
    for (var y = 0; y < h; y++)
    {
        for (var x = 0; x < w; x++)
        {
            var ix = (y * w) + x;
            if (templates[i][ix] == '#')
            {
                piece.Segments.Add(new BigPoint(x, h - (y + 1)));
            }
        }
    }

    pieces.Add(piece);
}

var moveSeq = new LoopList<int>(File.ReadAllText("input.txt").Select(c => c == '<' ? -1 : 1));

var pieceSeq = new LoopList<Piece>(pieces);

Piece? active = null;

var rocks = new HashSet<BigPoint>();
var fallen = 0L;
var limit = 1_000_000_000_000L;
//var limit = 2022;
var maxY = 0L;
var next = 2022;

var maxHeights = new Dictionary<long, long>
{
    [1] = 0,
    [2] = 0,
    [3] = 0,
    [4] = 0,
    [5] = 0,
    [6] = 0,
    [7] = 0,
};

var start = DateTime.Now;
var deltaH = 0L;

var steps = 0;

while (fallen < limit)
{
    if (fallen == next)
    {
        var reclaimed = rocks.RemoveWhere(r => r.Y + 1_000 < maxHeights[r.X]);
        var elapsed = (DateTime.Now - start).TotalSeconds;
        var speed = fallen / elapsed;
        var remaining = (limit - fallen) / speed;

        // Console.Write($"\rPieces: {fallen} ({speed:N2} per second) ({remaining:N2} seconds remaining). Height: {maxY}. Reclaimed: {reclaimed}");
        //Console.WriteLine($"Delta: {maxY - deltaH}");
        deltaH = maxY;

        next += 2022;
    }
    if (active == null)
    {
        var y = maxY + 4;
        active = pieceSeq.Next().Spawn(3, y);
    }
    else
    {
        active.Offset(0, -1);
        var hit = active.Segments.Any(s => s.Y < 1 || rocks.Contains(s));
        steps++;
        if (hit)
        {
            active.Offset(0, 1);

            foreach (var s in active.Segments)
            {
                maxY = Math.Max(s.Y, maxY);
                maxHeights[s.X] = Math.Max(maxHeights[s.X], s.Y);
                rocks.Add(s);
            }
            active = null;
            fallen++;
            Console.WriteLine($"Fallen: {fallen} ({steps} steps)");
            steps = 0;
            continue;
        }
    }

    var deltaX = moveSeq.Next();

    active.Offset(deltaX, 0);

    if (active.Segments.Any(s => s.X is < 1 or > 7))
    {
        active.Offset(-deltaX, 0);
    }
    else if (active.Segments.Any(rocks.Contains))
    {
        active.Offset(-deltaX, 0);
    }

    /*
    Console.Clear();
    for (var y = 0; y < Console.WindowHeight - 1; y++)
    {
        Console.SetCursorPosition(0, y);
        Console.Write("|       |");
    }
    Console.SetCursorPosition(0, Console.WindowHeight - 1);
    Console.Write($"+-------+ {fallen}");

    if (active != null)
    {
        foreach (var segment in active.Segments)
        {
            Console.SetCursorPosition((int)segment.X, Console.WindowHeight - (int)segment.Y - 1);
            Console.Write('#');
        }
    }

    foreach (var rock in rocks)
    {
        Console.SetCursorPosition((int)rock.X, Console.WindowHeight - (int)rock.Y - 1);
        Console.Write('@');
    }
    await Task.Delay(500);
    */
}

var part1 = rocks.Max(s => s.Y);
Console.WriteLine();
Console.WriteLine($"Part 1: {part1}");

internal class Piece
{
    public List<BigPoint> Segments = new();

    public Piece Spawn(long x, long y)
    {
        return new Piece { Segments = Segments.Select(s => s.OffsetCopy(x, y)).ToList() };
    }

    public Piece Offset(long x, long y)
    {
        foreach (var s in Segments)
        {
            s.Offset(x, y);
        }

        return this;
    }
}
