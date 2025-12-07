using System.Diagnostics.CodeAnalysis;

namespace AoC.Common;

public class Point(int x, int y)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;

    public Point Offset(int dx, int dy)
    {
        return new Point(X + dx, Y + dy);
    }

    public static Point Origin { get; } = new(0, 0);

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

public class BigPoint(long x, long y)
{
    public long X { get; set; } = x;
    public long Y { get; set; } = y;

    public static BigPoint Origin { get; } = new(0, 0);

    public static bool operator ==(BigPoint? p1, BigPoint? p2)
    {
        return p1 is null || p2 is null ? p1 is null && p2 is null : p1.X == p2.X && p1.Y == p2.Y;
    }

    public static bool operator !=(BigPoint? p1, BigPoint? p2)
    {
        return !(p1 == p2);
    }

    public static BigPoint operator +(BigPoint p1, BigPoint p2)
    {
        return new(p1.X + p2.X, p1.Y + p2.Y);
    }

    public static BigPoint operator -(BigPoint p1, BigPoint p2)
    {
        return new(p1.X - p2.X, p1.Y - p2.Y);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is BigPoint p && p == this;
    }

    public override int GetHashCode()
    {
        return (X ^ Y).GetHashCode();
    }

    public override string ToString()
    {
        return $"{X}x{Y}";
    }

    public BigPoint Offset(long x, long y)
    {
        X += x;
        Y += y;

        return this;
    }

    public BigPoint OffsetCopy(long x, long y)
    {
        return new BigPoint(X + x, Y + y);
    }
}

public class Edge<T>(T from, T to, bool unidirectional = false)
{
    public T From { get; } = from;
    public T To { get; } = to;
    public bool Unidirectional { get; } = unidirectional;
}

public class Edge(Point from, Point to, bool unidirectional = false) : Edge<Point>(from, to, unidirectional)
{
}
