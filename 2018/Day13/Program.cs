using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;

namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("Input.txt");
            var grid = Grid.Parse(input);
            using (var draw = new ImageDraw(grid.Width * 10 + 10, grid.Height * 10 + 10))
            {
                while (true)
                {
                    grid.Draw(draw);
                    draw.Recycle();
                    //Thread.Sleep(100);
                }
            }
        }
    }

    interface IDrawable
    {
        void Plot(int x, int y, char c);
        void Plot(int x, int y, string str);
    }

    class ImageDraw : IDrawable, IDisposable
    {
        int index;
        Bitmap img;
        Graphics g;
        //AnimatedGif.AnimatedGifCreator gif;
        Font font = new Font("FiraCode", 10);
        public ImageDraw(int w, int h)
        {
            img = new Bitmap(w, h);
            g = Graphics.FromImage(img);
            g.FillRectangle(Brushes.Black, new Rectangle(new Point(0, 0), img.Size));
            //gif = AnimatedGif.AnimatedGif.Create(@"c:\temp\output\2018.13.gif", 100, 0);
        }

        public void Recycle()
        {
            //gif.AddFrame(img);
            img.Save($@"c:\temp\output\{(index++).ToString("00000")}.png", ImageFormat.Png);
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
            g.DrawString(str, font, Brushes.White, (x * 10) - (size.Width / 2) + 5, y * 10);
        }

        public void Dispose()
        {
            img.Save($@"c:\temp\output\{(index++).ToString("00000")}.png", ImageFormat.Png);
            //gif.AddFrame(img);
            g.Dispose();
            img.Dispose();
        }
    }

    class Grid
    {
        public int Height => Cells.Keys.Max(k => k.y);
        public int Width => Cells.Keys.Max(k => k.x);
        public Cart[] Carts { get; set; }
        public Dictionary<(int x, int y), Cell> Cells { get; set; } = new Dictionary<(int x, int y), Cell>();
        public void Draw(IDrawable draw)
        {
            foreach(var cell in Cells.Values)
            {
                draw.Plot(cell.X, cell.Y, cell.CellType);
            }

            foreach(var cart in Carts)
            {
                draw.Plot(cart.X, cart.Y, cart.Render());
                cart.Update(Cells, Carts.Where(c => c != cart));
            }
        }
        public static Grid Parse(string input)
        {
            var carts = new List<Cart>();
            var cells = new List<Cell>();
            using (var reader = new StringReader(input))
            {
                string row;
                var y = 0;
                while(!string.IsNullOrEmpty(row = reader.ReadLine()))
                {
                    for(var x = 0; x < row.Length; x++)
                    {
                        var token = row[x];
                        if(Cart.IsCart(token))
                        {
                            carts.Add(Cart.Create(x, y, token));
                            cells.Add(Cell.Create(x, y, '-'));
                        }

                        if(Cell.IsCell(token))
                        {
                            cells.Add(Cell.Create(x, y, token));
                        }
                    }
                    y++;
                }
            }
            return new Grid
            {
                Carts = carts.ToArray(),
                Cells = cells.ToDictionary(c => (c.X, c.Y), c => c)
            };
        }
    }

    class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char CellType { get; set; }
        public static bool IsCell(char c) => new[] { '/', '\\', '-', '+', '|' }.Contains(c);
        public static Cell Create(int x, int y, char c) => new Cell { X = x, Y = y, CellType = c };
    }

    class Cart
    {
        private static Dictionary<char, Direction> dirMappings = new Dictionary<char, Direction>
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
        public char Render() => Crashed ? 'X' : dirMappings.First(kv => kv.Value == Facing).Key;
        public void Update(Dictionary<(int x, int y), Cell> cells, IEnumerable<Cart> otherCarts)
        {
            if(Crashed)
            {
                return;
            }
            X += 
                Facing == Direction.Left ? -1 :
                Facing == Direction.Right ? 1 : 0;

            Y += 
                Facing == Direction.Up ? -1 :
                Facing == Direction.Down ? 1 : 0;

            foreach(var cart in otherCarts)
            {
                if(cart.X != X || cart.Y != Y)
                {
                    continue;
                }
                cart.Crashed = true;
                Crashed = true;
            }

            if(Crashed)
            {
                Console.WriteLine($"{X},{Y}");
                return;
            }

            var cell = cells[(X, Y)];

            if(cell.CellType == '\\')
            {
                Facing =
                    Facing == Direction.Right ? Direction.Down :
                    Facing == Direction.Left ? Direction.Up :
                    Facing == Direction.Down ? Direction.Right :
                    Direction.Left;
            }

            if(cell.CellType == '/')
            {
                Facing =
                    Facing == Direction.Left ? Direction.Down :
                    Facing == Direction.Right ? Direction.Up :
                    Facing == Direction.Down ? Direction.Left :
                    Direction.Right;
            }

            if(cell.CellType == '+')
            {
                if(Intersection == 0)
                {
                    Facing =
                        Facing == Direction.Up ? Direction.Left :
                        Facing == Direction.Left ? Direction.Down :
                        Facing == Direction.Down ? Direction.Right :
                        Direction.Up;
                }
                if(Intersection == 2)
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

        public static bool IsCart(char c) => dirMappings.ContainsKey(c);
        public static Cart Create(int x, int y, char c) => new Cart { X = x, Y = y, Facing = dirMappings[c], Intersection = 0 };
    }
}
