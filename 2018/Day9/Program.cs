using System;
using System.Collections.Generic;
using System.Linq;
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
            var players = 10;
            var player = 0;
            var limit = 1618;
            var score = Enumerable.Range(0, players).ToDictionary(x => x, x => 0);
            while (next < limit + 1)
            {
                if (next % 23 == 0)
                {
                    var removed = (active + marbles.Count - 7) % marbles.Count + 1;
                    score[player] += 23 + marbles[removed];
                    marbles.RemoveAt(removed);
                    active = removed >= marbles.Count ? 0 : removed;
                }
                else
                {
                    var nextActive = (active + 2) % marbles.Count;
                    marbles.Insert(nextActive + 1, next);
                    active = nextActive;
                }

                //Console.WriteLine($"[{player + 1}] {string.Join(" ", marbles)}");
                player = (player + 1) % players;
                //Thread.Sleep(100);
                next++;
            }
            foreach(var item in score.OrderBy(kv => kv.Key))
            {
                Console.WriteLine($"Player {item.Key + 1}: {item.Value}");
            }
            Console.Read();
        }
    }
}
