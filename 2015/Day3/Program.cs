using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            var stack = new Queue<char>(File.ReadAllText("Input.txt").ToArray());

            var consumerCount = 1;

            var consumers = Enumerable.Range(0, consumerCount).Select(n => new Consumer()).ToList();

            var history = new HashSet<(int x, int y)>
            {
                (0, 0)
            };

            while (stack.Any())
            {
                history.AddRange(consumers.Select(c => c.Move(stack.Dequeue())));
            }

            Console.WriteLine(history.Count);

            Console.ReadLine();
        }
    }

    static class Extensions
    {
        public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> values)
        {
            foreach(var value in values)
            {
                set.Add(value);
            }
        }
    }

    class Consumer
    {
        readonly Pos pos = new Pos { X = 0, Y = 0 };
        public (int x, int y) Move(char ch)
        {
            if(ch == '^')
            {
                pos.Y++;
            }
            else if(ch == '>')
            {
                pos.X++;
            }
            else if(ch == 'v')
            {
                pos.Y--;
            }
            else if(ch == '<')
            {
                pos.X--;
            }
            else
            {
                throw new Exception();
            }

            return (pos.X, pos.Y);
        }
    }

    class Pos
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override int GetHashCode()
        {
            return $"{X}x{Y}".GetHashCode(); 
        }
    }
}
