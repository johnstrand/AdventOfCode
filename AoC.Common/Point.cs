using System.Diagnostics.CodeAnalysis;

namespace AoC.Common;
public class Point
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void Normalize()
    {
        X = Normalize(X);
        Y = Normalize(Y);
    }

    private static int Normalize(int value)
    {
        return value == 0 ? 0 : value / Math.Abs(value);
    }

    public static bool operator ==(Point? p1, Point? p2)
    {
        return p1 is null || p2 is null ? p1 is null && p2 is null : p1.X == p2.X && p1.Y == p2.Y;
    }

    public static bool operator !=(Point? p1, Point? p2)
    {
        return !(p1 == p2);
    }

    public static Point operator +(Point p1, Point p2)
    {
        return new(p1.X + p2.X, p1.Y + p2.Y);
    }

    public static Point operator -(Point p1, Point p2)
    {
        return new(p1.X - p2.X, p1.Y - p2.Y);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Point p && p == this;
    }

    public override int GetHashCode()
    {
        return unchecked(X ^ Y);
    }

    public override string ToString()
    {
        return $"{X}x{Y}";
    }
}
