using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day8
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = new Queue<string>(File.ReadAllText("Input.txt").Split(' '));
            var tree = ReadNode(input);
            Console.WriteLine($"Metadata sum: {tree.SimpleValue}");
            Console.WriteLine($"Complex metadata sum: {tree.ComplexValue}");
            Console.Read();
        }

        static Node ReadNode(Queue<string> data)
        {
            var childNodeCount = int.Parse(data.Dequeue());
            var metaDataCount = int.Parse(data.Dequeue());
            var children = Enumerable.Range(1, childNodeCount).Select(n => ReadNode(data)).ToList();

            return new Node
            {
                Children = children,
                Metadata = Enumerable.Range(1, metaDataCount).Select(n => int.Parse(data.Dequeue())).ToList()
            };
        }
    }
    class Node
    {
        public List<int> Metadata { get; set; }
        public List<Node> Children { get; set; }
        public int SimpleValue => Metadata.Sum() + Children.Select(c => c.SimpleValue).Sum();
        public int ComplexValue => Children.Any() ? Metadata.Where(v => v <= Children.Count).Select(v => Children[v - 1].ComplexValue).Sum() : SimpleValue;
    }
}
