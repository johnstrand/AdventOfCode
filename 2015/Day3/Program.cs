using Day03;

var stack = new Queue<char>(File.ReadAllText("Input.txt").ToArray());

var consumerCount = 1;

var consumers = Enumerable.Range(0, consumerCount).Select(_ => new Consumer()).ToList();

var history = new HashSet<(int x, int y)>
{
    (0, 0)
};

while (stack.Count > 0)
{
    history.AddRange(consumers.Select(c => c.Move(stack.Dequeue())));
}

Console.WriteLine(history.Count);

Console.ReadLine();

namespace Day03
{
    internal static class Extensions
    {
        public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                set.Add(value);
            }
        }
    }

    internal class Consumer
    {
        private readonly Pos _pos = new() { X = 0, Y = 0 };
        public (int x, int y) Move(char ch)
        {
            if (ch == '^')
            {
                _pos.Y++;
            }
            else if (ch == '>')
            {
                _pos.X++;
            }
            else if (ch == 'v')
            {
                _pos.Y--;
            }
            else if (ch == '<')
            {
                _pos.X--;
            }
            else
            {
                throw new Exception();
            }

            return (_pos.X, _pos.Y);
        }
    }

    internal class Pos
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override int GetHashCode()
        {
            return $"{X}x{Y}".GetHashCode();
        }
    }
}
