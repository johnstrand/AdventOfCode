using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            var points = GetPoints();

            var bounded = points.Where(p => IsBounded(p, points));
            
            var bounds = points.Aggregate<Point, Bounds>(null, (acc, cur) =>
            {
                if(acc == null)
                {
                    return new Bounds(cur.X, cur.Y, cur.X, cur.Y);
                }
                return acc.Modify(cur);
            });

            foreach(var bp in bounds.GetPoints())
            {
                Console.SetCursorPosition(bp.X, bp.Y);
                var n = Nearest(bp, points);
                if(n.HasValue)
                {
                    Console.Write("*");
                }
                else
                {
                    Console.Write(".");
                }
            }
            Console.Read();
        }

        static Point? Nearest(Point p, IEnumerable<Point> points)
        {
            var cluster = (from ptemp in points group ptemp by Distance(ptemp, p) into g select new { g.Key, Values = g.ToList() }).ToDictionary(x => x.Key, x => x.Values);
            var nearestKey = cluster.Keys.OrderBy(k => k).First();

            return cluster[nearestKey].Count == 1 ? cluster[nearestKey].First() : new Point?();
        }
        static int Distance(Point p1, Point p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
        }
        static bool IsBounded(Point p, IEnumerable<Point> points)
        {
            var boundTop = false;
            var boundBottom = false;
            var boundLeft = false;
            var boundRight = false;
            foreach(var point in points)
            {
                boundLeft = boundLeft || point.X < p.X;
                boundRight = boundRight || point.X > p.X;
                boundTop = boundTop || point.Y < p.Y;
                boundBottom = boundBottom || point.Y > p.Y;
            }

            return boundBottom && boundLeft && boundRight && boundTop;
        }

        static IEnumerable<Point> GetPoints()
        {
            return File.ReadAllLines(@"Input.txt").Select(row => row.Split(',').Select(int.Parse)).Select(parts => new Point(parts.First(), parts.Last()));
        }
    }

    class Bounds
    {
        public Bounds(int x1, int y1, int x2, int y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public Bounds Modify(Point p)
        {
            X1 = Math.Min(X1, p.X);
            X2 = Math.Max(X2, p.X);
            Y1 = Math.Min(Y1, p.Y);
            Y2 = Math.Max(Y2, p.Y);
            return this;
        }
        public bool Contains(Point p) => p.X >= X1 && p.X <= X2 && p.Y >= Y1 && p.Y <= Y2;

        public IEnumerable<Point> GetPoints()
        {
            for(var y = Y1; y <= Y2; y++)
            {
                for(var x = X1; x <= X2; x++)
                {
                    yield return new Point(x, y);
                }
            }
        }
    }
}
