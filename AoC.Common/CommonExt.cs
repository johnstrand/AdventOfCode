namespace AoC.Common;
public static class CommonExt
{
    public static string[] SplitRemoveEmpty(this string str)
    {
        return str.SplitRemoveEmpty(' ');
    }
    public static string[] SplitRemoveEmpty(this string str, params string[] delimiters)
    {
        return str.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
    }

    public static string[] SplitRemoveEmpty(this string str, params char[] delimiters)
    {
        return str.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
    }

    public static (string, string) ToTuple(this string str, char delimiter)
    {
        var parts = str.Split(delimiter);
        return (parts[0], parts[1]);
    }

    public static IEnumerable<int> ToNumbers32(this string str, char delimiter = ' ')
    {
        return str.SplitRemoveEmpty(delimiter).Select(s => int.TryParse(s, out var i) ? i : throw new Exception($"Could not parse '{s}' to int"));
    }

    public static IEnumerable<long> ToNumbers64(this string str, char delimiter = ' ')
    {
        return str.SplitRemoveEmpty(delimiter).Select(s => long.TryParse(s, out var i) ? i : throw new Exception($"Could not parse '{s}' to int"));
    }

    public static IEnumerable<int> ToNumbers32(this IEnumerable<string> strings)
    {
        return strings.Select(s => int.TryParse(s, out var i) ? i : throw new Exception($"Could not parse '{s}' to int"));
    }

    public static IEnumerable<int> ToRange(this (int from, int to) range)
    {
        return Enumerable.Range(range.from, range.to - range.from + 1);
    }

    public static IEnumerable<int> IndexesOf(this string str, char c)
    {
        var index = -1;

        while ((index = str.IndexOf(c, index + 1)) != -1)
        {
            yield return index;
        }
    }

    public static IEnumerable<int> IndexesOf(this char[] str, char c)
    {
        var index = -1;

        while ((index = Array.IndexOf(str, c, index + 1)) != -1)
        {
            yield return index;
        }
    }
}
