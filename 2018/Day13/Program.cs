// TODO: Fix me
using System.Drawing;

var input = File.ReadAllText("Input.txt");

var reader = new Board(input);

var grid = Grid.Parse(input);

using (var draw = new ImageDraw((grid.Width * 10) + 5, (grid.Height * 10) + 5))
{
    while (!grid.Draw(draw))
    {
        draw.Recycle();
        //Thread.Sleep(100);
    }
}

internal class Mover(int x, int y, Direction dir)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
    public Direction Dir { get; set; } = dir;

    private readonly Turn nextTurn = Turn.Left;

    public void Tick()
    {
    }
}

internal class Board
{
    private readonly int _w;
    private readonly int _h;
    private readonly Dictionary<(int x, int y), char> _data = [];
    private readonly Dictionary<(int x, int y), Room> _rooms = [];
    private readonly List<Mover> _movers = [];

    public Room GetRoom(int x, int y)
    {
        return _rooms[(x, y)];
    }

    public Board(string input)
    {
        using var reader = new StringReader(input);
        string row;
        for (var y = 0; !string.IsNullOrEmpty(row = reader.ReadLine()); y++)
        {
            _w = row.Length;
            _h++;
            for (var x = 0; x < row.Length; x++)
            {
                var c = row[x];
                if (c == ' ')
                {
                    continue;
                }

                _data[(x, y)] = c;
            }
        }

        foreach (var node in _data)
        {
            var (x, y) = node.Key;
            var c = node.Value;
            var connections = Connections.None;

            if (c == '/')
            {
                if (HasBlock(x, y, 0, 1, '|'))
                {
                    connections |= Connections.Right | Connections.Bottom;
                }
                else
                {
                    connections |= Connections.Top | Connections.Left;
                }
            }
            else if (c == '\\')
            {
                if (HasBlock(x, y, 0, 1, '|'))
                {
                    connections |= Connections.Bottom | Connections.Left;
                }
                else
                {
                    connections |= Connections.Top | Connections.Right;
                }
            }
            else if (c == '-')
            {
                connections |= Connections.Left | Connections.Right;
            }
            else if (c == '|')
            {
                connections |= Connections.Top | Connections.Bottom;
            }
            else if (c == '+')
            {
                connections |= Connections.All;
            }
            else
            {
                var dir = c switch
                {
                    '>' => Direction.Right,
                    '<' => Direction.Left,
                    '^' => Direction.Up,
                    _ => Direction.Down
                };

                _movers.Add(new(x, y, dir));

                if (HasBlock(x, y, 0, -1, '/', '\\', '|', '+'))
                {
                    connections |= Connections.Top;
                }

                if (HasBlock(x, y, 0, 1, '/', '\\', '|', '+'))
                {
                    connections |= Connections.Bottom;
                }

                if (HasBlock(x, y, -1, 0, '/', '\\', '-', '+'))
                {
                    connections |= Connections.Left;
                }

                if (HasBlock(x, y, 1, 0, '/', '\\', '-', '+'))
                {
                    connections |= Connections.Right;
                }
            }

            _rooms[node.Key] = new(x, y, connections);
        }
    }

    private bool HasBlock(int x, int y, int dx, int dy, params char[] block)
    {
        var (sx, sy) = (x + dx, y + dy);

        return IsInBounds(sx, sy) && _data.ContainsKey((sx, sy)) && block.Contains(_data[(sx, sy)]);
    }

    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && y >= 0 && x < _w && y < _h;
    }
}

internal class Room(int x, int y, Connections connections)
{
    public int X { get; } = x;
    public int Y { get; } = y;

    public Connections Connections { get; } = connections;
}

internal enum Turn
{
    Left,
    Straight,
    Right
}

internal enum Direction
{
    Up,
    Down,
    Left,
    Right
}

[Flags]
internal enum Connections
{
    None = 0,
    Top = 1,
    Right = 1 << 1,
    Bottom = 1 << 2,
    Left = 1 << 3,
    All = Top | Right | Bottom | Left
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

    private const int cellSize = 10;
    private const int halfCell = cellSize / 2;

    private Point TopLeft(int x, int y) => new(x * cellSize, y * cellSize);
    private Point TopCenter(int x, int y) => new((x * cellSize) + halfCell, y * cellSize);
    private Point TopRight(int x, int y) => new((x * cellSize) + cellSize - 1, y * cellSize);

    private Point Left(int x, int y) => new(x * cellSize, (y * cellSize) + halfCell);
    private Point Center(int x, int y) => new((x * cellSize) + halfCell, (y * cellSize) + halfCell);
    private Point Right(int x, int y) => new((x * cellSize) + cellSize - 1, (y * cellSize) + halfCell);

    private Point BottomLeft(int x, int y) => new(x * cellSize, (y * cellSize) + cellSize - 1);
    private Point BottomCenter(int x, int y) => new((x * cellSize) + halfCell, (y * cellSize) + cellSize - 1);
    private Point BottomRight(int x, int y) => new((x * cellSize) + cellSize - 1, (y * cellSize) + cellSize - 1);

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
    public Dictionary<(int x, int y), Cell> Cells { get; set; } = [];

    private readonly HashSet<(int x, int y)> crashMarkers = [];

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
        return Carts.Count(c => !c.Crashed) == 1;
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
            Carts = [.. carts],
            Cells = cells.ToDictionary(c => (c.X, c.Y), c => c)
        };
    }
}

internal class Cell
{
    public int X { get; set; }
    public int Y { get; set; }
    public char CellType { get; set; }

    internal static readonly char[] cellDelimiters = ['/', '\\', '-', '+', '|'];

    public static bool IsCell(char c)
    {
        return cellDelimiters.Contains(c);
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
