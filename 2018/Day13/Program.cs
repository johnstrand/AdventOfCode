// TODO: Fix me
using System.Drawing;

var input = File.ReadAllText("Input.txt");
var grid = Grid.Parse(input);
using (var draw = new ImageDraw((grid.Width * 5) + 5, (grid.Height * 5) + 5))
{
    while (true)
    {
        grid.Draw(draw);
        draw.Recycle();
        //Thread.Sleep(100);
    }
}

internal interface IDrawable
{
    void Plot(int x, int y, char c);
    void Plot(int x, int y, string str);
}

internal class ImageDraw : IDrawable, IDisposable
{
    //private readonly int index;
    private Bitmap img;
    private Graphics g;
    private readonly AnimatedGif.AnimatedGifCreator gif;
    private readonly Font font = new("FiraCode", 10);
    public ImageDraw(int w, int h)
    {
        img = new Bitmap(w, h);
        g = Graphics.FromImage(img);
        g.FillRectangle(Brushes.Black, new Rectangle(new Point(0, 0), img.Size));
        gif = AnimatedGif.AnimatedGif.Create(@"c:\temp\output\2018.13.gif", 100, 0);
    }

    public void Recycle()
    {
        gif.AddFrame(img);
        //img.Save($@"c:\temp\output\{(index++).ToString("00000")}.png", ImageFormat.Png);
        var w = img.Width;
        var h = img.Height;
        g.Dispose();
        img.Dispose();
        img = new Bitmap(w, h);
        g = Graphics.FromImage(img);
        g.FillRectangle(Brushes.Black, new Rectangle(new Point(0, 0), img.Size));
    }
    public void Plot(int x, int y, char c)
    {
        Plot(x, y, c.ToString());
    }

    public void Plot(int x, int y, string str)
    {
        var size = g.MeasureString(str, font);
        g.DrawString(str, font, Brushes.White, (x * 5) - (size.Width / 2) + 2, y * 5);
    }

    public void Dispose()
    {
        //img.Save($@"c:\temp\output\{(index++).ToString("00000")}.png", ImageFormat.Png);
        gif.AddFrame(img);
        g.Dispose();
        img.Dispose();
    }
}

internal class Grid
{
    public int Height => Cells.Keys.Max(k => k.y);
    public int Width => Cells.Keys.Max(k => k.x);
    public Cart[] Carts { get; set; }
    public Dictionary<(int x, int y), Cell> Cells { get; set; } = new Dictionary<(int x, int y), Cell>();

    private readonly HashSet<(int x, int y)> crashMarkers = new();
    public void Draw(IDrawable draw)
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
        }
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

        if (c1.Facing == Direction.Up && c1.X == c2.X && c1.Y + 1 == c1.Y)
        {
            return true;
        }
        if (c1.Facing == Direction.Down && c1.X == c2.X && c1.Y - 1 == c1.Y)
        {
            return true;
        }
        if (c1.Facing == Direction.Left && c1.X + 1 == c2.X && c1.Y == c1.Y)
        {
            return true;
        }
        if (c1.Facing == Direction.Right && c1.X - 1 == c2.X && c1.Y == c1.Y)
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
