namespace AoC.Common;
public static class CommonExt
{
    public static string[] SplitRemoveEmpty(this string str, params char[] delimiters)
    {
        return str.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
    }

    public static (string, string) ToTuple(this string str, char delimiter)
    {
        var parts = str.Split(delimiter);
        return (parts[0], parts[1]);
    }

    public static IEnumerable<int> ToNumbers(this string str, char delimiter = ' ')
    {
        return str.SplitRemoveEmpty(delimiter).Select(s => int.TryParse(s, out var i) ? i : throw new Exception($"Could not parse '{s}' to int"));
    }

    public static IEnumerable<int> ToNumbers(this IEnumerable<string> strings)
    {
        return strings.Select(s => int.TryParse(s, out var i) ? i : throw new Exception($"Could not parse '{s}' to int"));
    }

    public static IEnumerable<int> ToRange(this (int from, int to) range)
    {
        return Enumerable.Range(range.from, range.to - range.from + 1);
    }
}
