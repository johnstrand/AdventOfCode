using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day6
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Part1();
            Console.Read();
        }

        private static void Part1()
        {
            if (Directory.Exists("output1"))
            {
                Directory.Delete("output1", true);
            }
            var grid = new Grid(1000, 1000);
            grid.Register("turn on", (state, start, end) =>
            {
                For(start, end, p =>
                {
                    state[p.X, p.Y] = 1;
                });

                return state;
            });

            grid.Register("turn off", (state, start, end) =>
            {
                For(start, end, p =>
                {
                    state[p.X, p.Y] = 0;
                });

                return state;
            });

            grid.Register("toggle", (state, start, end) =>
            {
                For(start, end, p =>
                {
                    state[p.X, p.Y] = (byte)(state[p.X, p.Y] == 0 ? 1 : 0);
                });

                return state;
            });
            var frame = 1;
            foreach (var line in File.ReadAllLines("Input.txt"))
            {
                Console.Write($"Processing frame {frame.ToString("00000")}\r");
                var m = Regex.Match(line, @"^(?<command>.+) (?<from>.+?) through (?<to>.+?)$");
                var cmd = m.Groups["command"].Value;
                var from = ParsePoint(m.Groups["from"].Value);
                var to = ParsePoint(m.Groups["to"].Value);
                grid.Raise(cmd, from, to);
                //grid.AsBitmap().Save($"output1\\frame{frame.ToString("00000")}.png", ImageFormat.Png);
                frame++;
            }
            Console.WriteLine();
            Console.WriteLine(grid.Count(b => b > 0));
        }

        private static Point ParsePoint(string input)
        {
            var parts = input.Split(',').Select(int.Parse).ToList();
            return new Point(parts[0], parts[1]);
        }
        private static void For(Point start, Point end, Action<Point> action)
        {
            for (var y = start.Y; y <= end.Y; y++)
            {
                for (var x = start.X; x <= end.X; x++)
                {
                    action(new Point(x, y));
                }
            }
        }
    }

    internal class Grid
    {
        private byte[,] grid;
        private readonly int width;
        private readonly int height;
        private readonly Dictionary<string, Func<byte[,], Point, Point, byte[,]>> reducers = new Dictionary<string, Func<byte[,], Point, Point, byte[,]>>();
        public int Count(Func<byte, bool> counter)
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
            grid = new byte[width, height];
        }

        public void Register(string ev, Func<byte[,], Point, Point, byte[,]> reducer)
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
}
