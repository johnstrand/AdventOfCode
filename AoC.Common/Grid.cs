using System.Collections;

namespace AoC.Common;

public abstract class Grid
{
    public static Grid<int> FromRows(IEnumerable<string> rows)
    {
        return new Grid<int>(rows, row => row.Select(c => c - '0'));
    }

    public static Grid<T> FromRows<T>(IEnumerable<string> rows, Func<char, T> converter)
    {
        return new(rows, str => str.Select(converter));
    }
}

public class Grid<T> : Grid, IEnumerable<T>
{
    private readonly List<T> _items = [];

    public T this[int x, int y]
    {
        get => GetValue(x, y);
        set => SetValue(x, y, value);
    }

    public T this[Point p]
    {
        get => GetValue(p.X, p.Y);
        set => SetValue(p.X, p.Y, value);
    }

    public int Width { get; private set; }

    public int Height { get; private set; }

    public int Count => _items.Count;

    public string Stringified => string.Join(Environment.NewLine, Enumerable.Range(0, Height).Select(offset => string.Join(" ", _items.GetRange(offset * Width, Width))));

    public IEnumerable<(int x, int y)> Coordinates => Enumerable.Range(0, Height).SelectMany(y => Enumerable.Range(0, Width).Select(x => (x, y)));

    public Grid(int width, int height)
    {
        Width = width;
        Height = height;
        _items.AddRange(Enumerable.Repeat<T>(default!, width * height));
    }

    public Grid(int width, int height, IEnumerable<T> items)
    {
        Width = width;
        Height = height;
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
    }

    public void Expand(int w, int h, T? placeholder = default)
    {
        if (w > Width)
        {
            for (var index = Height - 1; index >= 0; index--)
            {
                _items.InsertRange((index * Width) + Width, Enumerable.Repeat(placeholder!, w - Width));
            }
            Width = w;
        }

        if (h > Height)
        {
            _items.AddRange(Enumerable.Repeat(placeholder!, (h - Height) * Width));
            Height = h;
        }
    }

    public Grid<T> Clone()
    {
        return new Grid<T>(Width, Height, _items.ToList());
    }

    public T GetValue(int x, int y)
    {
        return _items[GetIndex(x, y)];
    }

    public T GetValue((int x, int y) coord)
    {
        return _items[GetIndex(coord.x, coord.y)];
    }

    public T SetValue(int x, int y, Func<T, T> modifier)
    {
        return _items[GetIndex(x, y)] = modifier(_items[GetIndex(x, y)]);
    }

    public T SetValue(int x, int y, T value)
    {
        return _items[GetIndex(x, y)] = value;
    }

    public bool IsValid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < Width && y < Height;
    }

    public IEnumerable<T?> GetRow(int row)
    {
        if (!IsValid(0, row))
        {
            yield break;
        }

        for (var x = 0; x < Width; x++)
        {
            yield return GetValue(x, row);
        }
    }

    public IEnumerable<T?> GetColumn(int column)
    {
        if (!IsValid(column, 0))
        {
            yield break;
        }

        for (var y = 0; y < Height; y++)
        {
            yield return GetValue(column, y);
        }
    }

    public bool CompareColumns(int c1, int c2)
    {
        return IsValid(c1, 0) && IsValid(c2, 0) && (c1 == c2 || GetColumn(c1).SequenceEqual(GetColumn(c2)));
    }

    public int CountColumnsEqual(int c1, int c2)
    {
        var col1 = GetColumn(c1).ToList();

        if (c1 == c2)
        {
            return col1.Count;
        }

        var col2 = GetColumn(c2).ToList();

        var result = 0;
        for (var i = 0; i < col1.Count; i++)
        {
            if (Equals(col1[i], col2[i]))
            {
                result++;
            }
        }

        return result;
    }

    public bool CompareRows(int r1, int r2)
    {
        return IsValid(0, r1) && IsValid(0, r2) && (r1 == r2 || GetRow(r1).SequenceEqual(GetRow(r2)));
    }

    public int CountRowsEqual(int r1, int r2)
    {
        var row1 = GetRow(r1).ToList();

        if (r1 == r2)
        {
            return row1.Count;
        }

        var row2 = GetRow(r2).ToList();

        var result = 0;
        for (var i = 0; i < row1.Count; i++)
        {
            if (Equals(row1[i], row2[i]))
            {
                result++;
            }
        }

        return result;
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

    public void ForEach(Func<(int x, int y), T, T> callback)
    {
        var newValues = new List<T>();
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
        return (y * Width) + x;
    }

    public static Grid<T> FromRows(IEnumerable<string> rows, Func<char, T> mapper)
    {
        return new Grid<T>(rows, row => row.Select(mapper));
    }

    public void Display(int maxHeight = -1)
    {
        for (var y = 0; y < Height; y++)
        {
            if (y == maxHeight)
            {
                return;
            }

            for (var x = 0; x < Width; x++)
            {
                Console.Write(_items[GetIndex(x, y)]);
            }
            Console.WriteLine();
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return ((IEnumerable<T>)_items).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_items).GetEnumerator();
    }
}