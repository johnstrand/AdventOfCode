﻿// TODO: This is a clusterwhoopsie, go fix it

using System.Drawing;
using System.Drawing.Imaging;

Dictionary<string, Color> colorLookup = [];
Random r = new();
var points = GetPoints();

var bounded = new HashSet<string>(points.Where(p => IsBounded(p, points)).Select(p => p.Name));

var bounds = points.Aggregate<NamedPoint, Bounds>(null, (acc, cur) =>
    acc == null ? new Bounds(cur.X, cur.Y, cur.X, cur.Y) : acc.Modify(cur));

var img = new Bitmap(bounds.X2 - bounds.X1 + 1, bounds.Y2 - bounds.Y1 + 1);
var img2 = new Bitmap(bounds.X2 - bounds.X1 + 1, bounds.Y2 - bounds.Y1 + 1);

var areas = new Dictionary<string, int>();

var pts = bounds.GetPoints().ToList();
var index = 0;

var nearest = 0;
foreach (var bp in pts)
{
    Console.Write($"{++index} / {pts.Count}\r");
    if (points.Select(p => Distance(p, bp)).Sum() < 10000)
    {
        nearest++;
        img2.SetPixel(bp.X - bounds.X1, bp.Y - bounds.Y1, Color.Green);
    }
    else
    {
        img2.SetPixel(bp.X - bounds.X1, bp.Y - bounds.Y1, Color.Black);
    }
    //Console.SetCursorPosition(bp.X, bp.Y);
    var n = Nearest(bp, points);
    Color color;
    if (n != null)
    {
        if (bounded.Contains(n.Name))
        {
            if (!areas.TryGetValue(n.Name, out var value))
            {
                value = 0;
                areas.Add(n.Name, value);
            }
            areas[n.Name] = ++value;
        }
        color = GetColor(n.Name);
    }
    else
    {
        color = Color.DarkGray;
    }
    img.SetPixel(bp.X - bounds.X1, bp.Y - bounds.Y1, color);
}

img.Save(@"c:\temp\2018.06-1.png", ImageFormat.Png);
img2.Save(@"c:\temp\2018.06-2.png", ImageFormat.Png);
foreach (var area in areas.OrderBy(a => a.Value))
{
    Console.WriteLine($"{area.Key}: {area.Value}");
}
Console.WriteLine(nearest);
Console.Read();

Color GetColor(string name)
{
    if (!colorLookup.TryGetValue(name, out var value))
    {
        value = Color.FromArgb(r.Next(120, 256), r.Next(120, 256), r.Next(120, 256));
        colorLookup.Add(name, value);
    }
    return value;
}

NamedPoint Nearest(Point p, IEnumerable<NamedPoint> points)
{
    var cluster = (from ptemp in points group ptemp by Distance(ptemp, p) into g select new { g.Key, Values = g.ToList() }).ToDictionary(x => x.Key, x => x.Values);
    var nearestKey = cluster.Keys.OrderBy(k => k).First();

    return cluster[nearestKey].Count == 1 ? cluster[nearestKey].First() : null;
}

int Distance(NamedPoint p1, Point p2)
{
    return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
}

bool IsBounded(NamedPoint p, IEnumerable<NamedPoint> points)
{
    // Basically, this doesn't cover all situations, and I just got lucky
    var boundTop = false;
    var boundBottom = false;
    var boundLeft = false;
    var boundRight = false;
    foreach (var point in points)
    {
        boundLeft = boundLeft || point.X < p.X;
        boundRight = boundRight || point.X > p.X;
        boundTop = boundTop || point.Y < p.Y;
        boundBottom = boundBottom || point.Y > p.Y;
    }

    return boundBottom && boundLeft && boundRight && boundTop;
}

IEnumerable<NamedPoint> GetPoints()
{
    return File.ReadAllLines("Input.txt")
        .Select(row =>
            row.Split(',').Select(int.Parse))
        .Select((parts, index) =>
            new NamedPoint(parts.First(), parts.Last(), index.ToString("000")));
}

internal class NamedPoint(int x, int y, string name)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
    public string Name { get; set; } = name;
}

internal class Bounds(int x1, int y1, int x2, int y2)
{
    public int X1 { get; set; } = x1;
    public int Y1 { get; set; } = y1;
    public int X2 { get; set; } = x2;
    public int Y2 { get; set; } = y2;
    public Bounds Modify(NamedPoint p)
    {
        X1 = Math.Min(X1, p.X);
        X2 = Math.Max(X2, p.X);
        Y1 = Math.Min(Y1, p.Y);
        Y2 = Math.Max(Y2, p.Y);
        return this;
    }
    public bool Contains(Point p)
    {
        return p.X >= X1 && p.X <= X2 && p.Y >= Y1 && p.Y <= Y2;
    }

    public IEnumerable<Point> GetPoints()
    {
        for (var y = Y1; y <= Y2; y++)
        {
            for (var x = X1; x <= X2; x++)
            {
                yield return new Point(x, y);
            }
        }
    }
}
