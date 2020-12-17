using System;
using System.Collections.Generic;

var grid = new Grid().Load(3, 3, ".#...####");

Console.WriteLine(grid.GetValue(1, 0, 0));

class Grid
{
    private readonly Dictionary<Point, char> points = new Dictionary<Point, char>();

    public IEnumerable<char> GetNeighbors(int x, int y, int z)
    {
        foreach (var tz in new[] { z - 1, z, z + 1 })
        {
            foreach (var ty in new[] { y - 1, y, y + 1 })
            {
                foreach (var tx in new[] { x - 1, x, x + 1 })
                {
                    if (tz == z && ty == y && tx == x)
                    {
                        continue;
                    }
                    yield return GetValue(x, y, z);
                }
            }
        }
    }

    public Grid Load(int w, int h, string source)
    {
        for (var y = 0; y < h; y++)
        {
            for (var x = 0; x < w; x++)
            {
                SetValue(x, y, 0, source[y * w + x] == '#');
            }
        }

        return this;
    }

    public char GetValue(int x, int y, int z)
    {
        var point = new Point(x, y, z);
        if (!points.ContainsKey(point))
        {
            points[point] = '.';
        }

        return points[point];
    }

    public Grid SetValue(int x, int y, int z, bool set)
    {
        var point = new Point(x, y, z);
        points[point] = set ? '#' : '.';
        return this;
    }
}

record Point(int X, int Y, int Z);
