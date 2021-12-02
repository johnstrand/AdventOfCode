// See https://aka.ms/new-console-template for more information
using System;
using System.IO;

var pos = (x: 0, y: 0);
foreach (var line in File.ReadAllLines("input.txt"))
{
    var parts = line.Split(' ');
    var dir = parts[0];
    var dist = int.Parse(parts[1]);
    pos = dir switch
    {
        "forward" => (pos.x + dist, pos.y),
        "down" => (pos.x, pos.y + dist),
        "up" => (pos.x, pos.y - dist),
        _ => pos
    };

    Console.WriteLine(pos);
}

Console.WriteLine($"Part 1: {pos.x * pos.y}");
