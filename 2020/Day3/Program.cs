using System;
using System.IO;

var rows = File.ReadAllLines(@"input.txt");
var steppings = new[]
{
    (dx: 1, dy: 1),
    (dx: 3, dy: 1),
    (dx: 5, dy: 1),
    (dx: 7, dy: 1),
    (dx: 1, dy: 2),
};
var w = rows[0].Length;
var product = 1;
foreach (var (dx, dy) in steppings)
{
    var x = 0;
    var y = 0;
    var trees = 0;
    while ((y += dy) < rows.Length)
    {
        x += dx;
        if (rows[y][x % w] == '#')
        {
            trees++;
        }
    }
    product *= trees;
    Console.WriteLine($"Stepping ({dx}, {dy}) = {trees}");
}

Console.WriteLine($"Product: {product}");
