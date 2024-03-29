﻿// TODO: Also not complete
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

var points = File.ReadAllLines("Input.txt").Select(Point.Parse).ToList();
var frame = 0;
while (true)
{
    var img = new Bitmap(500, 500);
    var visible = points.All(p => p.Visible(img));
    if (visible)
    {
        Graphics.FromImage(img).Pipe(g => g.FillRectangle(Brushes.Black, 0, 0, 1000, 1000)).Dispose();
        Console.Write($"[{frame}]\r");
    }
    foreach (var point in points)
    {
        if (point.Visible(img))
        {
            img.SetPixel(point.X, point.Y, Color.White);
        }
        point.Update();
    }
    frame++;
    if (visible)
    {
        img.Save($@"C:\temp\output\{frame:00000}.png", ImageFormat.Png);
        img.Dispose();
    }
}

internal static class Extensions
{
    public static T Pipe<T>(this T o, Action<T> action)
    {
        action(o);
        return o;
    }
}

internal class Point(int x, int y, int dx, int dy)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
    public int Dx { get; set; } = dx;
    public int Dy { get; set; } = dy;
    public bool Visible(Image img) => X >= 0 && Y >= 0 && X < img.Width && Y < img.Height;
    public void Update()
    {
        X += Dx;
        Y += Dy;
    }
    public static Point Parse(string row)
    {
        var m = Regex.Match(row, "position=<(?<pos>.+?)> velocity=<(?<vel>.+?)>");
        var pos = m.Groups["pos"].Value.Split(',').Select(int.Parse);
        var vel = m.Groups["vel"].Value.Split(',').Select(int.Parse);
        return new Point(pos.First(), pos.Last(), vel.First(), vel.Last());
    }
}
