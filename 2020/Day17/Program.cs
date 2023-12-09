using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var grid = new Grid(File.ReadAllLines("input.txt"));

for (var cycle = 1; cycle <= 6; cycle++)
{
    Console.WriteLine($"Cycle: {cycle}");
    grid.Display();
    grid.Step();
    Console.WriteLine();
}
Console.WriteLine($"Active: {grid.Active}");

internal class Grid
{
    private HashSet<(int x, int y, int z)> points = [];

    public int Active => points.Count;

    private bool IsActive(int x, int y, int z)
    {
        return points.Contains((x, y, z));
    }

    private int Adjacent(int x, int y, int z)
    {
        return (
            from tz in Enumerable.Range(z - 1, 3)
            from ty in Enumerable.Range(y - 1, 3)
            from tx in Enumerable.Range(x - 1, 3)
            where !(tx == x && ty == y && tz == z) && IsActive(tx, ty, tz)
            select 1).Sum();
    }

    public void Display()
    {
        GetMinMax(out int minZ, out int minY, out int minX, out int maxZ, out int maxY, out int maxX);
        for (var z = minZ; z <= maxZ; z++)
        {
            Console.WriteLine($"z={z}");
            for (var y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    var active = IsActive(x, y, z);
                    Console.Write(active ? "#" : " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

    public void Step()
    {
        var newPoints = new HashSet<(int x, int y, int z)>();
        GetMinMax(out int minZ, out int minY, out int minX, out int maxZ, out int maxY, out int maxX);
        for (var z = minZ - 1; z <= maxZ + 1; z++)
        {
            for (var y = minY - 1; y <= maxY + 1; y++)
            {
                for (int x = minX - 1; x <= maxX + 1; x++)
                {
                    var active = IsActive(x, y, z);
                    var adj = Adjacent(x, y, z);
                    //Console.WriteLine($"({x}, {y}, {z}) = {active}/{adj}");
                    var nextActive = (active && adj is 2 or 3) || (!active && adj == 3);
                    if (nextActive)
                    {
                        newPoints.Add((x, y, z));
                    }
                }
            }
        }

        points = newPoints;
    }

    private void GetMinMax(out int minZ, out int minY, out int minX, out int maxZ, out int maxY, out int maxX)
    {
        minZ = minY = minX = int.MaxValue;
        maxZ = maxY = maxX = int.MinValue;
        foreach (var (x, y, z) in points)
        {
            if (x < minX)
            {
                minX = x;
            }

            if (y < minY)
            {
                minY = y;
            }

            if (z < minZ)
            {
                minZ = z;
            }

            if (x > maxX)
            {
                maxX = x;
            }

            if (y > maxY)
            {
                maxY = y;
            }

            if (z > maxZ)
            {
                maxZ = z;
            }
        }
    }

    public Grid(string[] baseState)
    {
        for (var y = 0; y < baseState.Length; y++)
        {
            for (var x = 0; x < baseState[y].Length; x++)
            {
                if (baseState[y][x] != '#')
                {
                    continue;
                }
                points.Add((x, y, 0));
            }
        }
    }
}