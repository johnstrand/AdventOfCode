var wires = new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };

var t = new[] { 1, 2, 3 };
foreach(var iter in GetIterations(wires))
{
    Console.WriteLine(string.Join(", ", iter));
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

    for (var iter = 0; iter < _items.Count; iter++)
    {
        for (var i = 0; i < _items.Count - 1; i++)
        {
            yield return _items;
            Swap(_items, i, i + 1);
        }
    }
}

bool IsValid(List<char> connections, char[] active)
{
    
}