using System.Diagnostics.CodeAnalysis;

namespace AoC.Common;

public class Point3
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    public static Point3 Origin { get; } = new(0, 0, 0);

    public Point3(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public void Normalize()
    {
        X = Normalize(X);
        Y = Normalize(Y);
        Z = Normalize(Z);
    }

    private static int Normalize(int value)
    {
        return value == 0 ? 0 : value / Math.Abs(value);
    }

    public static bool operator ==(Point3? p1, Point3? p2)
    {
        return p1 is null || p2 is null ? p1 is null && p2 is null : p1.X == p2.X && p1.Y == p2.Y && p1.Z == p2.Z;
    }

    public static bool operator !=(Point3? p1, Point3? p2)
    {
        return !(p1 == p2);
    }

    public static Point3 operator +(Point3 p1, Point3 p2)
    {
        return new(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
    }

    public static Point3 operator -(Point3 p1, Point3 p2)
    {
        return new(p1.X - p2.X, p1.Y - p2.Y, p1.Z + p2.Z);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Point3 p && p == this;
    }

    public override int GetHashCode()
    {
        return unchecked(X ^ Y ^ Z);
    }

    public override string ToString()
    {
        return $"{X}x{Y}x{Z}";
    }
}
