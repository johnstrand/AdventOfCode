namespace AoC.Common;
public class LoopList<T>
{
    private readonly List<T> _list;

    public int Count => _list.Count;
    public int Index { get; private set; }

    public LoopList(in IEnumerable<T> list)
    {
        _list = list.ToList();
        if (_list.Count == 0)
        {
            throw new ArgumentException("Supplied empty was empty", nameof(list));
        }
    }

    public T Next()
    {
        var value = _list[Index];
        Index = (Index + 1) % _list.Count;
        return value;
    }
}
