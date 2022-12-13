using System.Collections;
using System.Text;

var source = new Queue<string>(File.ReadLines("input-test.txt"));
var source2 = new List<object>();
var part1 = 0;
var index = 0;
while (source.Count > 0)
{
    index++;
    Console.WriteLine($"Index {index}:");
    try
    {
        var left = Parser.Parse(source.Dequeue()).ToList();
        var right = Parser.Parse(source.Dequeue()).ToList();
        source2.Add(left);
        source2.Add(right);
        if (Comparer.Compare(left, right) ?? true)
        {
            part1 += index;
            Console.WriteLine("OK");
        }
        else
        {
            Console.WriteLine("Not OK");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Not OK: {ex.Message}");
    }
    if (source.Count > 0)
    {
        source.Dequeue(); // Consume empty line
    }
    Console.WriteLine();
}


var t = Sorter.Sort(source2).ToList();
Console.WriteLine($"Part 1: {part1}");

static class Sorter
{
    public static IEnumerable<object> Sort(IEnumerable<object> source)
    {
        return source.OrderBy(item => item is List<object> ls ? ls.Count : (int)item);
    }
}

static class Comparer
{
    public static bool? Compare(object left, object right, int depth = 0)
    {
        var a = new Queue<object>((List<object>)left);
        var b = new Queue<object>((List<object>)right);

        Console.WriteLine($"{Pad(depth)}- {Stringify(a)} vs {Stringify(b)}");

        while (a.Count > 0 && b.Count > 0)
        {
            var v1 = a.Dequeue();
            var v2 = b.Dequeue();

            if (v1 is int l && v2 is int r)
            {
                Console.WriteLine($"{Pad(depth + 1)}- {Stringify(l)} vs {Stringify(r)}");
                if (l > r)
                {
                    Console.WriteLine($"{Pad(depth + 2)}- {l} is greater than {r}: Fail");
                    return false;
                }

                if (l < r)
                {
                    Console.WriteLine($"{Pad(depth + 2)}- {l} is less than {r}: OK");
                    return true;
                }

            }
            else
            {
                var check = Compare(EnsureList(v1), EnsureList(v2), depth + 1);
                if (check.HasValue)
                {
                    return check.Value;
                }
            }

        }
        if (a.Count > 0)
        {
            return false;
        }
        if (b.Count > 0)
        {
            return true;
        }

        return null;
    }

    //CompareLists((List<object>)left, (List<object>)right);

    private static string Pad(int len)
    {
        return new string(' ', len);
    }

    private static object EnsureList(object value)
    {
        return value is IEnumerable ? value : new List<object> { value };
    }

    private static void CompareLists(List<object> left, List<object> right, int depth = 0)
    {
        Console.WriteLine($"{new string(' ', depth)}- Compare {Stringify(left)} - {Stringify(right)}");
        for (var i = 0; i < left.Count; i++)
        {
            if (i >= right.Count)
            {
                throw new("Right list ran out of items");
            }

            var a = left[i];
            var b = right[i];
            a = Normalize(a, b is IEnumerable);
            b = Normalize(b, a is IEnumerable);

            if (a is int v1 && b is int v2)
            {
                Console.WriteLine($"{new string(' ', depth + 1)}- Compare {Stringify(a)} - {Stringify(b)}");
                if (v1 > v2)
                {
                    throw new($"{v1} is greater than {v2}");
                }
                if (v1 == v2)
                {
                    continue;
                }

                return;
            }

            CompareLists((List<object>)a, (List<object>)b, depth + 1);
        }
    }

    private static object Normalize(object value, bool otherIsList)
    {
        return value.GetType() == typeof(int) && otherIsList ? new List<object> { value } : value;
    }

    private static string Stringify(object value)
    {
        return value is List<object> ls ? $"[{string.Join(", ", ls.Select(Stringify))}]" : value.ToString()!;
    }
}

static class Parser
{
    public static IEnumerable<object> Parse(string source)
    {
        var q = new Queue<char>(source);
        return ParseList(q);
    }

    private static IEnumerable<object> ParseList(Queue<char> source)
    {
        source.Dequeue(); // Remove leading [
        while (source.Count > 0)
        {
            if (source.Peek() == ']')
            {
                break;
            }

            yield return source.Peek() == '[' ? ParseList(source).ToList() : ParseNumber(source);

            if (source.Peek() == ',')
            {
                source.Dequeue(); // Remove ,
            }
        }
        source.Dequeue(); // Remove trailing ]
    }

    private static object ParseNumber(Queue<char> source)
    {
        var num = new StringBuilder();
        while (char.IsNumber(source.Peek()))
        {
            num.Append(source.Dequeue());
        }
        return int.Parse(num.ToString());
    }
}
