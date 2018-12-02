using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        static void Part1()
        {
            var grid = new Grid(1000, 1000);
            grid.Register("on", (state, start, end) =>
            {
                For(start, end, p =>
                {
                    state[p.X, p.Y] = true;
                });

                return state;
            });
        }

        static void For(Point start, Point end, Action<Point> action)
        {
            for(var y = start.Y; y <= end.Y; y++)
            {
                for(var x = start.X; x <= end.X; x++)
                {
                    action(new Point(x, y));
                }
            }
        }
    }

    class Grid
    {
        bool[,] grid;
        readonly int width;
        readonly int height;
        readonly Dictionary<string, Func<bool[,], Point, Point, bool[,]>> reducers = new Dictionary<string, Func<bool[,], Point, Point, bool[,]>>();
        public Grid(int width, int height)
        {
            this.width = width;
            this.height = height;
            grid = new bool[width, height];
        }

        public void Register(string ev, Func<bool[,], Point, Point, bool[,]> reducer)
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
            for(var y = 0; y < height; y++)
            {
                for(var x = 0; x < width; x++)
                {
                    img.SetPixel(x, y, grid[x, y] ? Color.White : Color.Black);
                }
            }
            return img;
        }
    }
}
