using System.Collections.Concurrent;

namespace AoC.Common;

public class Grid<T>
{
    private static readonly ConcurrentDictionary<(int x, int y), int> IndexCache = new();

    private readonly List<T?> _items = new();

    public int Width { get; }
    public int Height { get; }

    public int Count { get; }

    public Grid(int width, int height)
    {
        Width = width;
        Height = height;
        Count = width * height;
        _items.AddRange(Enumerable.Repeat<T?>(default, width * height));
    }

    public Grid(int width, int height, IEnumerable<T> items)
    {
        Width = width;
        Height = height;
        Count = width * height;
        _items.AddRange(items);
    }

    public Grid(IEnumerable<string> items, Func<string, IEnumerable<T>> mapper)
    {
        foreach (var line in items)
        {
            Height++;
            var mappedItems = mapper(line).ToList();
            Width = mappedItems.Count;
            _items.AddRange(mappedItems);
        }
        Count = Width * Height;
    }

    public T? GetValue(int x, int y)
    {
        return _items[GetIndex(x, y)];
    }

    public T? SetValue(int x, int y, Func<T?, T?> modifier)
    {
        return _items[GetIndex(x, y)] = modifier(_items[GetIndex(x, y)]);
    }

    public T? SetValue(int x, int y, T? value)
    {
        return _items[GetIndex(x, y)] = value;
    }

    public bool IsValid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < Width && y < Height;
    }

    public IEnumerable<(int x, int y)> GetMatching(Func<T?, bool> predicate)
    {
        for (var index = 0; index < _items.Count; index++)
        {
            if (predicate(_items[index]))
            {
                var x = index % Width;
                var y = index / Width;

                yield return (x, y);
            }
        }
    }

    public IEnumerable<(int x, int y)> GetAdjacent(int x, int y)
    {
        for (var ty = y - 1; ty <= y + 1; ty++)
        {
            for (var tx = x - 1; tx <= x + 1; tx++)
            {
                if ((tx != x || ty != y) && IsValid(tx, ty))
                {
                    yield return (tx, ty);
                }
            }
        }
    }

    public void ForEach(Func<(int x, int y), T?, T?> callback)
    {
        var newValues = new List<T?>();
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                newValues.Add(callback((x, y), GetValue(x, y)));
            }
        }
        _items.Clear();
        _items.AddRange(newValues);
    }

    private int GetIndex(int x, int y)
    {
        return IndexCache.GetOrAdd((x, y), (pos) => (pos.y * Width) + pos.x);
    }

    public static Grid<int> FromRows(IEnumerable<string> rows)
    {
        return new Grid<int>(rows, row => row.Select(c => c - '0'));
    }

    public void Display()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                Console.Write(_items[GetIndex(x, y)]);
            }
            Console.WriteLine();
        }
    }
}