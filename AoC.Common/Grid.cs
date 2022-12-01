using System.Collections.Concurrent;

namespace AoC.Common
{
    public class Grid<T>
    {
        private readonly static ConcurrentDictionary<(int x, int y), int> indexCache = new();

        private readonly List<T?> _items = new();

        public int Width { get; }
        public int Height { get; }

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
            _items.AddRange(Enumerable.Repeat<T?>(default, width * height));
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

        public T? GetValue(int x, int y)
        {
            return _items[GetIndex(x, y)];
        }

        public T? SetValue(int x, int y, T? value)
        {
            return _items[GetIndex(x, y)] = value;
        }

        public bool IsValid(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
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
            return indexCache.GetOrAdd((x, y), (pos) => pos.y * Width + pos.x);
        }

        public static Grid<int> FromRows(IEnumerable<string> rows)
        {
            return new Grid<int>(rows, row => row.Select(c => c - '0'));
        }
    }
}