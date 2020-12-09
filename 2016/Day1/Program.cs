using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

var pos = new Point(0, 0);
var facing = 0;
var seen = new HashSet<Point>();
var first = true;
foreach (var command in File.ReadAllText("input.txt").Split(',').Select(c => c.Trim()))
{
    if (command[0] == 'R')
    {
        facing = (facing + 1) % 4;
    }
    else
    {
        facing--;
        if (facing == -1)
        {
            facing = 3;
        }
    }

    var dist = int.Parse(command.Substring(1));
    for (var step = 0; step < dist; step++)
    {
        pos.Offset(
            facing == 1 ? 1 : facing == 3 ? -1 : 0,
            facing == 0 ? -1 : facing == 2 ? 1 : 0);

        if (first && !seen.Add(pos))
        {
            Console.WriteLine($"Seen twice: {Math.Abs(pos.X) + Math.Abs(pos.Y)}");
            first = false;
        }
    }
}

Console.WriteLine($"Distance: {Math.Abs(pos.X) + Math.Abs(pos.Y)}");
Console.Read();