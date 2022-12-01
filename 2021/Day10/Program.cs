var pairs = new[]
{
    (start: '(', end: ')'),
    (start: '<', end: '>'),
    (start: '{', end: '}'),
    (start: '[', end: ']'),
};

var values = new Dictionary<char, int>()
{
    [')'] = 3,
    [']'] = 57,
    ['}'] = 1197,
    ['>'] = 25137
};

var completeValues = new Dictionary<char, int>()
{
    [')'] = 1,
    [']'] = 2,
    ['}'] = 3,
    ['>'] = 4
};

var score = 0;
var subscores = new List<long>();
foreach (var line in File.ReadAllLines("input.txt"))
{
    var s = new Stack<char>();
    foreach (var c in line)
    {
        Console.Write(c);
        var (start, end) = pairs.First(p => p.start == c || p.end == c);
        if (start == c)
        {
            s.Push(end);
        }
        else
        {
            var n = s.Pop();
            if (n != end)
            {
                Console.Write($" *** corrupted (illegal character '{end}')");
                score += values[end];
                s.Clear();
                break;
            }
        }
    }
    if (s.Count > 0)
    {
        Console.Write(" *** incomplete");
        var subscore = 0L;
        while (s.Count > 0)
        {
            subscore = (subscore * 5) + completeValues[s.Pop()];
        }
        Console.WriteLine($" (subscore: {subscore})");
        subscores.Add(subscore);
    }
    Console.WriteLine();
}

subscores.Sort();

Console.WriteLine($"Part 1: {score}");
/*
for (var i = 0; i < subscores.Count; i++)
{
    Console.WriteLine($"[{i}] = {subscores[i]}");
}
*/
Console.WriteLine($"Part 2: {subscores[subscores.Count / 2]}");