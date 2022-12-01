// TODO: Fix me
using System.Drawing;

var input = File.ReadAllText("Input.txt");

var reader = new RoomReader(input);

var grid = Grid.Parse(input);

using (var draw = new ImageDraw((grid.Width * 10) + 5, (grid.Height * 10) + 5))
{
    while (!grid.Draw(draw))
    {
        draw.Recycle();
        //Thread.Sleep(100);
    }
}

class RoomReader
{
    private readonly List<char> data = new();
    private readonly int w;
    private readonly int h;
    private readonly Dictionary<(int x, int y), Room> roomCache = new();

    public RoomReader(string input)
    {
        data = new List<char>();
        using var reader = new StringReader(input);
        string row;
        while (!string.IsNullOrEmpty(row = reader.ReadLine()))
        {
            w = row.Length;
            h++;
            data.AddRange(row);
        }

        var start = data.FindIndex(c => c != ' ');
        var x = start % w;
        var y = start / h;
        Discover(x, y);
    }

    private Room Discover(int x, int y)
    {
        var key = (x, y);

        if (roomCache.TryGetValue(key, out var room))
        {
            return room;
        }

        room = roomCache[key] = new(x, y);

        var index = y * w + h;
        var c = data[index];

        if (c == '/')
        {
            if (HasBlock(x, y, 0, 1, '|'))
            {
                room.Bottom = Discover(x, y + 1);
                room.Right = Discover(x + 1, y);
            }
            else
            {
                room.Top = Discover(x, y - 1);
                room.Left = Discover(x - 1, y);
            }
        }
        else if (c == '\\')
        {
            if (HasBlock(x, y, 0, 1, '|'))
            {
                room.Bottom = Discover(x, y + 1);
                room.Left = Discover(x - 1, y);
            }
            else
            {
                room.Top = Discover(x, y - 1);
                room.Right = Discover(x + 1, y);
            }
        }
        else if (c == '-')
        {
            room.Left = Discover(x - 1, y);
            room.Right = Discover(x + 1, y);
        }
        else if (c == '|')
        {
            room.Top = Discover(x, y - 1);
            room.Bottom = Discover(x, y + 1);
        }
        else if (c == '+')
        {
            room.Left = Discover(x - 1, y);
            room.Right = Discover(x + 1, y);
            room.Top = Discover(x, y - 1);
            room.Bottom = Discover(x, y + 1);
        }
        else
        {
            if (HasBlock(x, y, 0, -1, '/', '\\', '|', '+'))
            {
                room.Top = Discover(x, y - 1);
            }

            if (HasBlock(x, y, 0, 1, '/', '\\', '|', '+'))
            {
                room.Bottom = Discover(x, y + 1);
            }

            if (HasBlock(x, y, -1, 0, '/', '\\', '-', '+'))
            {
                room.Left = Discover(x - 1, y);
            }

            if (HasBlock(x, y, 1, 0, '/', '\\', '-', '+'))
            {
                room.Right = Discover(x + 1, y);
            }

        }

        return room;
    }

    private bool HasBlock(int x, int y, int dx, int dy, params char[] block)
    {
        var (sx, sy) = (x + dx, y + dy);

        return IsInBounds(sx, sy) && block.Contains(data[sy * w + sx]);
    }

    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && y >= 0 && x < w && y < h;
    }
}

class Room
{
    public int X { get; }
    public int Y { get; }

    public Room Top { get; set; }
    public Room Bottom { get; set; }
    public Room Left { get; set; }
    public Room Right { get; set; }

    public Room(int x, int y)
    {
        X = x;
        Y = y;
    }
}

[Flags]
enum Connections
{
    Top = 1,
    Right = 2,
    Bottom = 4,
    Left = 8
}


internal interface IDrawable
{
    void Plot(int x, int y, char c);
}

internal class ImageDraw : IDrawable, IDisposable
{
    //private readonly int index;
    private Bitmap _img;
    private Graphics _g;
    private readonly AnimatedGif.AnimatedGifCreator gif;

    public ImageDraw(int w, int h)
    {
        _img = new Bitmap(w, h);
        _g = Graphics.FromImage(_img);
        _g.FillRectangle(Brushes.Black, new Rectangle(new Point(0, 0), _img.Size));
        gif = AnimatedGif.AnimatedGif.Create(@"c:\temp\output\2018.13.gif", 100, 0);
    }

    public void Recycle()
    {
        gif.AddFrame(_img);
        //img.Save($@"c:\temp\output\{(index++).ToString("00000")}.png", ImageFormat.Png);
        var w = _img.Width;
        var h = _img.Height;
        _g.Dispose();
        _img.Dispose();
        _img = new Bitmap(w, h);
        _g = Graphics.FromImage(_img);
        _g.FillRectangle(Brushes.Black, new Rectangle(new Point(0, 0), _img.Size));
    }
    public void Plot(int x, int y, char c)
    {
        if (c == '/')
        {
            _g.DrawLine(Pens.White, BottomLeft(x, y), TopRight(x, y));
        }
        else if (c == '\\')
        {
            _g.DrawLine(Pens.White, TopLeft(x, y), BottomRight(x, y));
        }
        else if (c == '-')
        {
            _g.DrawLine(Pens.White, Left(x, y), Right(x, y));
        }
        else if (c == '|')
        {
            _g.DrawLine(Pens.White, TopCenter(x, y), BottomCenter(x, y));
        }
        else if (c == '+')
        {
            _g.DrawLine(Pens.White, Left(x, y), Right(x, y));
            _g.DrawLine(Pens.White, TopCenter(x, y), BottomCenter(x, y));
        }
        else if (c == '>')
        {
            _g.DrawLine(Pens.Red, TopLeft(x, y), Right(x, y));
            _g.DrawLine(Pens.Red, BottomLeft(x, y), Right(x, y));
        }
        else if (c == '<')
        {
            _g.DrawLine(Pens.Red, Left(x, y), TopRight(x, y));
            _g.DrawLine(Pens.Red, Left(x, y), BottomRight(x, y));
        }
        else if (c == 'v')
        {
            _g.DrawLine(Pens.Red, TopLeft(x, y), BottomCenter(x, y));
            _g.DrawLine(Pens.Red, TopRight(x, y), BottomCenter(x, y));
        }
        else if (c == '^')
        {
            _g.DrawLine(Pens.Red, BottomRight(x, y), TopCenter(x, y));
            _g.DrawLine(Pens.Red, BottomLeft(x, y), TopCenter(x, y));
        }
        else if (c == 'X')
        {
            _g.DrawLine(Pens.Red, BottomRight(x, y), TopLeft(x, y));
            _g.DrawLine(Pens.Red, BottomLeft(x, y), TopRight(x, y));
        }
    }

    const int cellSize = 10;
    const int halfCell = cellSize / 2;

    private Point TopLeft(int x, int y) => new(x * cellSize, y * cellSize);
    private Point TopCenter(int x, int y) => new(x * cellSize + halfCell, y * cellSize);
    private Point TopRight(int x, int y) => new(x * cellSize + cellSize - 1, y * cellSize);

    private Point Left(int x, int y) => new(x * cellSize, y * cellSize + halfCell);
    private Point Center(int x, int y) => new(x * cellSize + halfCell, y * cellSize + halfCell);
    private Point Right(int x, int y) => new(x * cellSize + cellSize - 1, y * cellSize + halfCell);

    private Point BottomLeft(int x, int y) => new(x * cellSize, y * cellSize + cellSize - 1);
    private Point BottomCenter(int x, int y) => new(x * cellSize + halfCell, y * cellSize + cellSize - 1);
    private Point BottomRight(int x, int y) => new(x * cellSize + cellSize - 1, y * cellSize + cellSize - 1);


    public void Dispose()
    {
        //img.Save($@"c:\temp\output\{(index++).ToString("00000")}.png", ImageFormat.Png);
        gif.AddFrame(_img);
        _g.Dispose();
        _img.Dispose();
    }
}

internal class Grid
{
    public int Height => Cells.Keys.Max(k => k.y);
    public int Width => Cells.Keys.Max(k => k.x);
    public Cart[] Carts { get; set; }
    public Dictionary<(int x, int y), Cell> Cells { get; set; } = new Dictionary<(int x, int y), Cell>();

    private readonly HashSet<(int x, int y)> crashMarkers = new();

    public bool Draw(IDrawable draw)
    {
        foreach (var cell in Cells.Values)
        {
            draw.Plot(cell.X, cell.Y, cell.CellType);
        }

        foreach (var cart in Carts.Where(c => !c.Crashed))
        {
            draw.Plot(cart.X, cart.Y, cart.Render());
            cart.Update(Cells);
        }

        for (var index = 0; index < Carts.Length - 1; index++)
        {
            for (var sindex = index + 1; sindex < Carts.Length; sindex++)
            {
                if (Cart.HasCrashed(Carts[index], Carts[sindex]))
                {
                    Carts[index].Crashed = true;
                    Carts[sindex].Crashed = true;
                    if (crashMarkers.Add((Carts[index].X, Carts[index].Y)))
                    {
                        Console.WriteLine($"{Carts[index].X}x{Carts[index].Y}");
                    }
                    continue;
                }
            }
        }

        foreach (var (x, y) in crashMarkers)
        {
            draw.Plot(x, y, 'X');
        }
        //var collided = Carts.Where(c => !c.Crashed).GroupBy(c => (c.X, c.Y)).Where(g => g.Count() > 1);

        /*if(collided.Any())
        {
            foreach(var pos in collided)
            {
                if (crashMarkers.Add(pos.Key))
                {
                    Console.WriteLine($"{pos.Key.X}x{pos.Key.Y}");
                }
            }
            foreach(var c in collided.SelectMany(c => c))
            {
                c.Crashed = true;
            }
            Console.WriteLine(Carts.Count(c => !c.Crashed));
        }*/
        if (Carts.Count(c => !c.Crashed) == 1)
        {
            return true;
        }

        return false;
    }
    public static Grid Parse(string input)
    {
        var carts = new List<Cart>();
        var cells = new List<Cell>();
        using (var reader = new StringReader(input))
        {
            string row;
            for (var y = 0; !string.IsNullOrEmpty(row = reader.ReadLine()); y++)
            {
                for (var x = 0; x < row.Length; x++)
                {
                    var token = row[x];
                    if (Cart.IsCart(token))
                    {
                        carts.Add(Cart.Create(x, y, token));
                        cells.Add(Cell.Create(x, y, '-'));
                    }

                    if (Cell.IsCell(token))
                    {
                        cells.Add(Cell.Create(x, y, token));
                    }
                }
            }
        }
        return new Grid
        {
            Carts = carts.ToArray(),
            Cells = cells.ToDictionary(c => (c.X, c.Y), c => c)
        };
    }
}

internal class Cell
{
    public int X { get; set; }
    public int Y { get; set; }
    public char CellType { get; set; }
    public static bool IsCell(char c)
    {
        return new[] { '/', '\\', '-', '+', '|' }.Contains(c);
    }

    public static Cell Create(int x, int y, char c)
    {
        return new Cell { X = x, Y = y, CellType = c };
    }
}

internal class Cart
{
    private static readonly Dictionary<char, Direction> dirMappings = new()
    {
        { 'v', Direction.Down },
        { '^', Direction.Up },
        { '<', Direction.Left },
        { '>', Direction.Right }
    };

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public int X { get; set; }
    public int Y { get; set; }
    public Direction Facing { get; set; }
    public int Intersection { get; set; }
    public bool Crashed { get; set; }
    public char Render()
    {
        return Crashed ? 'X' : dirMappings.First(kv => kv.Value == Facing).Key;
    }

    public void Update(Dictionary<(int x, int y), Cell> cells)
    {
        if (Crashed)
        {
            return;
        }
        X +=
            Facing == Direction.Left ? -1 :
            Facing == Direction.Right ? 1 : 0;

        Y +=
            Facing == Direction.Up ? -1 :
            Facing == Direction.Down ? 1 : 0;

        var cell = cells[(X, Y)];

        if (cell.CellType == '\\')
        {
            Facing =
                Facing == Direction.Right ? Direction.Down :
                Facing == Direction.Left ? Direction.Up :
                Facing == Direction.Down ? Direction.Right :
                Direction.Left;
        }

        if (cell.CellType == '/')
        {
            Facing =
                Facing == Direction.Left ? Direction.Down :
                Facing == Direction.Right ? Direction.Up :
                Facing == Direction.Down ? Direction.Left :
                Direction.Right;
        }

        if (cell.CellType == '+')
        {
            if (Intersection == 0)
            {
                Facing =
                    Facing == Direction.Up ? Direction.Left :
                    Facing == Direction.Left ? Direction.Down :
                    Facing == Direction.Down ? Direction.Right :
                    Direction.Up;
            }
            if (Intersection == 2)
            {
                Facing =
                    Facing == Direction.Up ? Direction.Right :
                    Facing == Direction.Right ? Direction.Down :
                    Facing == Direction.Down ? Direction.Left :
                    Direction.Up;
            }
            Intersection = (Intersection + 1) % 3;
        }
    }
    public static bool HasCrashed(Cart c1, Cart c2)
    {
        if (c1.X == c2.X && c1.Y == c2.Y)
        {
            return true;
        }
        else if (c1.Facing == Direction.Up && c1.X == c2.X && c1.Y + 1 == c1.Y)
        {
            return true;
        }
        else if (c1.Facing == Direction.Down && c1.X == c2.X && c1.Y - 1 == c1.Y)
        {
            return true;
        }
        else if (c1.Facing == Direction.Left && c1.X + 1 == c2.X && c1.Y == c1.Y)
        {
            return true;
        }
        else if (c1.Facing == Direction.Right && c1.X - 1 == c2.X && c1.Y == c1.Y)
        {
            return true;
        }
        return false;
    }
    public static bool IsCart(char c)
    {
        return dirMappings.ContainsKey(c);
    }

    public static Cart Create(int x, int y, char c)
    {
        return new Cart { X = x, Y = y, Facing = dirMappings[c], Intersection = 0 };
    }
}
