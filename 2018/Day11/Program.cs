using System;
using System.Linq;

namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            var grid = (from x in Enumerable.Range(1, 300)
                        from y in Enumerable.Range(1, 300)
                        select new { x, y, p = CalculatePowerLevel(x, y, 9110) }).ToList();
            Console.Read();
        }

        static int CalculatePowerLevel(int x, int y, int serialNo)
        {
            var rackId = x + 10;
            var powerLevel = rackId * y;
            powerLevel += serialNo;
            powerLevel *= rackId;

            return (powerLevel / 100) % 10 - 5;
        }
    }
}
