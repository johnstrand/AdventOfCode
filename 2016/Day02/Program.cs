using System;
using System.IO;
using System.Linq;

var steps = File.ReadAllLines("input.txt").SelectMany(row => row + "E").ToList();
var grid1 = new[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };

var grid2 = new[]
{
    ' ', ' ', '1', ' ', ' ',
    ' ', '2', '3', '4', ' ',
    '5', '6', '7', '8', '9',
    ' ', 'A', 'B', 'C', ' ',
    ' ', ' ', 'D', ' ', ' '
};

var p1 = new Pos(1, 1, 3, 3, grid1);
foreach (var step in steps)
{
    if (step == 'E')
    {
        Console.Write(p1.Push());
    }
    else
    {
        p1.Move(step);
    }
}
Console.WriteLine();

var p2 = new Pos(2, 2, 5, 5, grid2);

foreach (var step in steps)
{
    if (step == 'E')
    {
        Console.Write(p2.Push());
    }
    else
    {
        p2.Move(step);
    }
}
Console.WriteLine();

class Pos
{
    private int x;
    private int y;
    private readonly int w;
    private readonly int h;
    private readonly char[] grid;

    public Pos(int x, int y, int w, int h, char[] grid)
    {
        this.x = x;
        this.y = y;
        this.w = w;
        this.h = h;
        this.grid = grid;
    }

    public void Move(char dir)
    {
        var ny = y + (dir == 'U' ? -1 : dir == 'D' ? 1 : 0);
        var nx = x + (dir == 'L' ? -1 : dir == 'R' ? 1 : 0);
        if (ny < 0 || ny == h || nx < 0 || nx == w)
        {
            return;
        }
        if (grid[ny * w + nx] == ' ')
        {
            return;
        }
        x = nx;
        y = ny;
    }

    public char Push()
    {
        return grid[y * w + x];
    }
}
