using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day23
{
    class Program
    {
        static void Main(string[] args)
        {
            var bots = File.ReadAllLines("Input.txt").Select(Bot.Parse).ToList();

            var max = bots.MaxValue(b => b.Radius);
            var inRangeCount = 0;
            foreach(var bot in bots)
            {
                var dist = max.Distance(bot);
                var inRange = dist <= max.Radius;
                if(inRange)
                {
                    inRangeCount++;
                }
            }

            Console.WriteLine($"In range: {inRangeCount}");

            var maxx = bots.Max(b => b.X);
            var maxy = bots.Max(b => b.Y);
            var maxz = bots.Max(b => b.Z);

            var minx = bots.Min(b => b.X);
            var miny = bots.Min(b => b.Y);
            var minz = bots.Min(b => b.Z);
            Console.Read();
        }
    }

    static class Extensions
    {
        public static TValue MaxValue<TValue, TSelect>(this IEnumerable<TValue> list, Func<TValue, TSelect> comp)
        {
            if(!(list?.Any() ?? false))
            {
                return default(TValue);
            }

           

            var selected = list.First();
            foreach(var item in list)
            {
                if(Comparer<TValue>.Default.Compare(item, selected) > 0)
                {
                    selected = item;
                }
            }

            return selected;
        }
    }

    class Bot : IComparable<Bot>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Radius { get; set; }

        public static Bot Parse(string row)
        {
            //pos=<0,0,0>, r=4
            var num = @"-?\d+";
            var m = Regex.Match(row, $"^pos=<(?<x>{num}),(?<y>{num}),(?<z>{num})>, r=(?<radius>{num})$");
            return new Bot
            {
                X = int.Parse(m.Groups["x"].Value),
                Y = int.Parse(m.Groups["y"].Value),
                Z = int.Parse(m.Groups["z"].Value),
                Radius = int.Parse(m.Groups["radius"].Value)
            };
        }

        public int CompareTo(Bot other)
        {
            return Radius.CompareTo(other.Radius);
        }

        public int Distance(Bot b)
        {
            return Math.Abs(b.X - X) + Math.Abs(b.Y - Y) + Math.Abs(b.Z - Z);
        }
    }
}
