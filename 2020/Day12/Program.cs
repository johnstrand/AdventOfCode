using System;
using System.Drawing;
using System.IO;

var b = new Boat();
foreach (var line in File.ReadAllLines("input.txt"))
{
    b.Move(line);
}
Console.WriteLine($"Part 1: {b.Distance}");
var wp = new Waypoint { X = 10, Y = 1 };
var ship = new Point(0, 0);
foreach (var line in File.ReadAllLines("input.txt"))
{
    Console.Write($"[{line}] ");
    if (line[0] == 'F')
    {
        var distance = int.Parse(line[1..]);
        Console.Write($"Ship moved from {Math.Abs(ship.X)} {(ship.X >= 0 ? "E" : "W")}, {Math.Abs(ship.Y)} {(ship.Y >= 0 ? "N" : "S")} to ");
        ship.X += (wp.X * distance);
        ship.Y += (wp.Y * distance);
        Console.WriteLine($"{Math.Abs(ship.X)} {(ship.X >= 0 ? "E" : "W")}, {Math.Abs(ship.Y)} {(ship.Y >= 0 ? "N" : "S")}");
    }
    else
    {
        wp.Move(line);
    }
}
Console.WriteLine($"Part 2: {Math.Abs(ship.X) + Math.Abs(ship.Y)}");

internal class Waypoint
{
    public int X { get; set; }
    public int Y { get; set; }

    public void Move(string instr)
    {
        var code = instr[0];
        var arg = int.Parse(instr[1..]);
        Console.Write($"Waypoint moved from {Math.Abs(X)} {(X >= 0 ? "E" : "W")}, {Math.Abs(Y)} {(Y >= 0 ? "N" : "S")} to ");
        switch (code)
        {
            case 'N':
                Y += arg;
                break;

            case 'S':
                Y -= arg;
                break;

            case 'E':
                X += arg;
                break;

            case 'W':
                X -= arg;
                break;

            case 'L':
                (X, Y) = (-Y, X);
                break;

            case 'R':
                (X, Y) = (Y, -X);
                break;

            default:
                throw new Exception(instr);
        }
        Console.WriteLine($"{Math.Abs(X)} {(X >= 0 ? "E" : "W")}, {Math.Abs(Y)} {(Y >= 0 ? "N" : "S")}");
    }
}

internal class Boat
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Vector { get; private set; } = 90;

    public int Distance => Math.Abs(X) + Math.Abs(Y);

    public void Move(string instr)
    {
        var code = instr[0];
        var arg = int.Parse(instr[1..]);
        switch (code)
        {
            case 'N':
                Y += arg;
                break;

            case 'S':
                Y -= arg;
                break;

            case 'E':
                X += arg;
                break;

            case 'W':
                X -= arg;
                break;

            case 'L':
                Vector -= arg;
                if (Vector < 0)
                {
                    Vector = 360 + Vector;
                }
                break;

            case 'R':
                Vector = (Vector + arg) % 360;
                break;

            case 'F' when Vector == 0:
                Y += arg;
                break;

            case 'F' when Vector == 90:
                X += arg;
                break;

            case 'F' when Vector == 180:
                Y -= arg;
                break;

            case 'F' when Vector == 270:
                X -= arg;
                break;

            default:
                throw new Exception(instr);
        }
    }
}