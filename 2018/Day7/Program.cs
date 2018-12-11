using System;

namespace Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    class Step
    {
        public string Name { get; set; }
        public bool Resolved { get; set; }
        public string ResolvedBy { get; set; }
    }
}
