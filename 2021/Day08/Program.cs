var wires = new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };

var numbers = new Dictionary<int, char[]>()
{
    [0] = new[] { 'a', 'b', 'c', 'e', 'f', 'g' },
    [1] = new[] { 'c', 'f' },
    [2] = new[] { 'a', 'c', 'd', 'e', 'g' },
    [3] = new[] { 'a', 'c', 'd', 'f', 'g' },
    [4] = new[] { 'b', 'c', 'd', 'f' },
    [5] = new[] { 'a', 'b', 'd', 'f', 'g' },
    [6] = new[] { 'a', 'b', 'd', 'e', 'f', 'g' },
    [7] = new[] { 'a', 'c', 'f' },
    [8] = new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g' },
    [9] = new[] { 'a', 'b', 'c', 'd', 'f', 'g' },
};

var part1 = 0;
var part2 = 0;
foreach (var row in File.ReadAllLines("input.txt"))
{
    var segments = row.Split('|');
    var signalsPatterns = segments[0].Trim().Split(' ');
    var output = segments[1].Trim().Split(' ');

    foreach (var iter in GetPerms(wires))
    {
        if (signalsPatterns.All(s => IsValid(iter, s.ToArray())))
        {
            DrawMappedSequences(iter, signalsPatterns);
            Console.ForegroundColor = ConsoleColor.Green;
            DrawMappedSequences(iter, output);
            Console.ResetColor();
            var outputNumbers = output.Select(s => GetNumber(iter, s.ToArray())).ToList();
            part1 += outputNumbers.Count(n => n is 1 or 4 or 7 or 8);
            part2 += outputNumbers[0] * 1000 + outputNumbers[1] * 100 + outputNumbers[2] * 10 + outputNumbers[3];
            break;
        }
    }
}

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

IEnumerable<List<T>> GetPerms<T>(IEnumerable<T> items)
{
    var _items = items.ToList();
    if (!_items.Any())
    {
        yield return _items;
        yield break;
    }
    for (var index = 0; index < _items.Count; index++)
    {
        var first = _items[index];
        foreach (var rest in GetPerms(WithoutIndex(_items, index)))
        {
            yield return rest.Prepend(first).ToList();
        }
    }
}

List<T> WithoutIndex<T>(List<T> items, int index)
{
    var copy = new List<T>(items);
    copy.RemoveAt(index);
    return copy;
}

IEnumerable<List<T>> GetIterations<T>(IEnumerable<T> items)
{
    var _items = new List<T>(items);

    void Swap(List<T> arr, int ix1, int ix2)
    {
        var t = arr[ix1];
        arr[ix1] = arr[ix2];
        arr[ix2] = t;
    }

    for (var x = 0; x < 2; x++)
    {
        for (var iter = 0; iter < _items.Count; iter++)
        {
            for (var i = 0; i < _items.Count - 1; i++)
            {
                yield return _items;
                Swap(_items, i, i + 1);
            }
        }
    }
}

void DrawMappedSequences(List<char> connections, params string[] sequences)
{
    var mappings = connections.Zip(wires).ToDictionary(kv => kv.First, kv => kv.Second);
    var bar = "  ****  ";
    var side = " *      ";
    var rside = new string(side.Reverse().ToArray());

    var msequences = sequences.Select(seq => string.Concat(seq.Select(c => mappings[c]))).ToArray();

    Each(msequences, ('a', bar));
    Each(msequences, ('b', side), ('c', rside));
    Each(msequences, ('b', side), ('c', rside));
    Each(msequences, ('d', bar));
    Each(msequences, ('e', side), ('f', rside));
    Each(msequences, ('e', side), ('f', rside));
    Each(msequences, ('g', bar));
    Console.WriteLine();
}

void DrawSequences(params string[] sequences)
{
    var bar = "  ****  ";
    var side = " *      ";
    var rside = new string(side.Reverse().ToArray());

    Each(sequences, ('a', bar));
    Each(sequences, ('b', side), ('c', rside));
    Each(sequences, ('b', side), ('c', rside));
    Each(sequences, ('d', bar));
    Each(sequences, ('e', side), ('f', rside));
    Each(sequences, ('e', side), ('f', rside));
    Each(sequences, ('g', bar));
    Console.WriteLine();
}

void Each(string[] sequences, params (char cond, string? value)[] values)
{
    foreach (var sequence in sequences)
    {
        Console.Write(Mix(9, values.Select(v => sequence.Contains(v.cond) ? v.value : null).ToArray()));
    }
    Console.WriteLine();
}

string Mix(int length, params string?[] strings)
{
    var data = Enumerable.Repeat(' ', length).ToArray();
    foreach (var s in strings)
    {
        if (s == null)
        {
            continue;
        }
        for (var index = 0; index < s.Length; index++)
        {
            if (s[index] != ' ')
            {
                data[index] = s[index];
            }
        }
    }
    return new string(data);
}

int GetNumber(List<char> connections, char[] active)
{
    var mappings = connections.Zip(wires).ToDictionary(kv => kv.First, kv => kv.Second);
    var mapped = active.Select(a => mappings[a]).ToList();

    return numbers.FirstOrDefault(kv => kv.Value.Length == mapped.Count && kv.Value.All(mapped.Contains)).Key;
}

bool IsValid(List<char> connections, char[] active)
{
    var mappings = connections.Zip(wires).ToDictionary(kv => kv.First, kv => kv.Second);
    var mapped = active.Select(a => mappings[a]).ToList();

    return numbers.Any(kv => kv.Value.Length == mapped.Count && kv.Value.All(mapped.Contains));
}
