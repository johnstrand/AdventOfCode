using System.Diagnostics.CodeAnalysis;

namespace AoC.Common;
public class Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public static Point Origin { get; } = new(0, 0);

    public Point(int x, int y)
    {
        X = x;
        Y = y;
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

public class BigPoint
{
    public long X { get; set; }
    public long Y { get; set; }

    public static BigPoint Origin { get; } = new(0, 0);

    public BigPoint(long x, long y)
    {
        X = x;
        Y = y;
    }

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

public class Edge<T>
{
    public T From { get; }
    public T To { get; }
    public bool Unidirectional { get; }

    public Edge(T from, T to, bool unidirectional = false)
    {
        From = from;
        To = to;
        Unidirectional = unidirectional;
    }
}

public class Edge : Edge<Point>
{
    public Edge(Point from, Point to, bool unidirectional = false) : base(from, to, unidirectional) { }
}
