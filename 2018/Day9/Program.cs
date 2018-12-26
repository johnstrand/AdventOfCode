using System;
using System.Collections.Generic;
using System.Threading;

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            var marbles = new List<int> { 0 };
            var active = 0;
            var next = 1;
            while (next < 26)
            {
                if (next % 23 == 0)
                {
                    var removed = (active + marbles.Count - 7) % marbles.Count + 1;
                    marbles.RemoveAt(removed);
                    active = removed >= marbles.Count ? 0 : removed;
                }
                else
                {
                    var nextActive = (active + 2) % marbles.Count;
                    marbles.Insert(nextActive + 1, next);
                    active = nextActive;
                }
                Console.WriteLine(string.Join(" ", marbles));
                Thread.Sleep(1000);
                next++;
            }
            Console.Read();
        }
    }
}
