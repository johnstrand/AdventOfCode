using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day6
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var lookup = new Dictionary<string, Body>
            {
                { "COM", new Body { Name = "COM", Depth = 0 } }
            };

            var q = new Queue<string[]>(File.ReadAllLines("input.txt").Select(r => r.Split(')')));
            while (q.Any())
            {
                var row = q.Dequeue();
                if (!lookup.ContainsKey(row[0]))
                {
                    q.Enqueue(row);
                    continue;
                }
                var p = lookup[row[0]];
                var c = new Body { Name = row[1], Depth = p.Depth + 1, Parent = p };
                p.Children.Add(c);
                lookup.Add(c.Name, c);
            }

            Console.WriteLine(lookup.Sum(kv => kv.Value.Depth));

            var me = lookup["YOU"];
            var steps = 0;
            while (!me.FindChildRef("SAN"))
            {
                me = me.Parent;
                steps++;
            }

            while (me.Name != "SAN")
            {
                me = me.Children.First(c => c.FindChildRef("SAN"));
                steps++;
            }

            Console.WriteLine(steps - 2);
        }
    }

    internal class Body
    {
        public string Name { get; set; }
        public int Depth { get; set; }
        public Body Parent { get; set; }
        public List<Body> Children { get; set; } = new List<Body>();

        public bool FindChildRef(string name)
        {
            if (Name == name)
            {
                return true;
            }

            return Children.Any(child => child.FindChildRef(name));
        }
    }
}