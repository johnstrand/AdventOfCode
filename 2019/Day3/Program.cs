using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Day3
{
    internal class Program
    {
        private static readonly Dictionary<Point, int> nodeCount = new Dictionary<Point, int>();

        private static void Main(string[] args)
        {
            var wires = File.ReadAllLines("input.txt");

            using var window = new RenderWindow(new SFML.Window.VideoMode(800, 600), "Advent of Code");
            using var dotTexture = new Texture("dot.png");
            using var dot = new Sprite(dotTexture);
            var view = window.DefaultView;
            view.Move(new SFML.System.Vector2f(-400, -300));
            window.SetView(view);
            window.Closed += (s, e) => window.Close();
            Task.Run(() =>
            {
                foreach (var wire in wires)
                {
                    Eval(wire.Split(',').ToList(), 0, 0, dot);
                }

                /*
                var minx = nodeCount.Min(n => n.Key.X);
                var miny = nodeCount.Min(n => n.Key.Y);
                var maxx = nodeCount.Max(n => n.Key.X);
                var maxy = nodeCount.Max(n => n.Key.Y);
                */
                var min = nodeCount.Keys.Where(n => nodeCount[n] > 1 && n != new Point(0, 0)).Min(n => Math.Abs(n.X) + Math.Abs(n.Y));
                Console.WriteLine(min);
            });
            while (window.IsOpen)
            {
                //window.Clear();
                window.DispatchEvents();
                window.Draw(dot);
                window.Display();
            }
        }

        private static void Eval(List<string> instr, int x, int y, Sprite dot)
        {
            if (!instr.Any())
            {
                return;
            }

            var cur = instr.First();
            var dir = cur[0];
            var dist = int.Parse(cur.Substring(1));

            var getOffset = new Func<string, int>(axis =>
            {
                if ((axis == "y" && (dir == 'L' || dir == 'R')) || (axis == "x" && (dir == 'U' || dir == 'D')))
                {
                    return 0;
                }

                return (dir == 'L' || dir == 'D') ? -1 : 1;
            });

            while (dist > -1)
            {
                x += getOffset("x");
                y += getOffset("y");

                var key = new Point(x, y);
                dot.Color = nodeCount.ContainsKey(key) ? Color.Red : Color.White;
                dot.Position = new SFML.System.Vector2f(x, y);
                if (nodeCount.ContainsKey(key))
                {
                    nodeCount[key]++;
                    Console.WriteLine(key);
                }
                else
                {
                    nodeCount.Add(key, 1);
                }
                dist--;

                Task.Delay(10).Wait();
            }

            Eval(instr.Skip(1).ToList(), x, y, dot);
        }
    }

    internal struct Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X;
        public int Y;

        public static bool operator ==(Point p1, Point p2) => p1.X == p2.X && p1.Y == p2.Y;

        public static bool operator !=(Point p1, Point p2) => p1.X != p2.X || p1.Y != p2.Y;

        public override bool Equals(object obj)
        {
            return obj != null && (obj is Point p) && p == this;
        }

        public override int GetHashCode()
        {
            return (X * Y).GetHashCode();
        }

        public override string ToString()
        {
            return $" {X} , {Y} ";
        }
    }
}