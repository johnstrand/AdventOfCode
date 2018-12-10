using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            var points = File.ReadAllLines("Input.txt").Select(Point.Parse).ToList();
            var frame = 0;
            while(true)
            {
                var img = new Bitmap(500, 500);
                var visible = points.All(p => p.Visible(img));
                if (visible)
                {
                    Graphics.FromImage(img).Pipe(g => g.FillRectangle(Brushes.Black, 0, 0, 1000, 1000)).Dispose();
                    Console.Write($"[{frame}]\r");
                }
                foreach(var point in points)
                {
                    if(point.Visible(img))
                    {
                        img.SetPixel(point.X, point.Y, Color.White);
                    }
                    point.Update();
                }
                frame++;
                if (visible)
                {
                    img.Save($@"C:\temp\output\{frame.ToString("00000")}.png", ImageFormat.Png);
                    img.Dispose();
                }
            }
        }
    }
    static class Extensions
    {
        public static T Pipe<T>(this T o, Action<T> action)
        {
            action(o);
            return o;
        }
    }
    class Point
    {
        public Point(int x, int y, int dx, int dy)
        {
            X = x;
            Y = y;
            Dx = dx;
            Dy = dy;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public int Dx { get; set; }
        public int Dy { get; set; }
        public bool Visible(Image img) => X >= 0 && Y >= 0 && X < img.Width && Y < img.Height;
        public void Update()
        {
            X += Dx;
            Y += Dy;
        }
        public static Point Parse(string row)
        {
            var m = Regex.Match(row, @"position=<(?<pos>.+?)> velocity=<(?<vel>.+?)>");
            var pos = m.Groups["pos"].Value.Split(',').Select(int.Parse);
            var vel = m.Groups["vel"].Value.Split(',').Select(int.Parse);
            return new Point(pos.First(), pos.Last(), vel.First(), vel.Last());
        }
    }
}
