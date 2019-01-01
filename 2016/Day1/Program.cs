using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Day1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var pos = new Point(0, 0);
            var facing = 0;
            var seen = new HashSet<Point>();
            foreach (var command in File.ReadAllText("Input.txt").Split(',').Select(c => c.Trim()))
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

                pos.Offset(
                    facing == 1 ? dist : facing == 3 ? -dist : 0,
                    facing == 0 ? -dist : facing == 2 ? dist : 0);

                if (!seen.Add(pos))
                {
                    Console.WriteLine($"Seen twice: {Math.Abs(pos.X) + Math.Abs(pos.Y)}");
                }
            }

            Console.WriteLine($"Distance: {Math.Abs(pos.X) + Math.Abs(pos.Y)}");
            Console.Read();
        }
    }
}
