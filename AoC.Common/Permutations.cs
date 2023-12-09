namespace AoC.Common;
public static class Permutations
{
    public static IEnumerable<List<T>> Generate<T>(List<T> values)
    {
        return Generate(values, int.MaxValue);
    }

    public static IEnumerable<List<T>> Generate<T>(List<T> values, int max)
    {
        static IEnumerable<List<T>> PermuteInner(List<T> accumulator, List<T> candidates, int max)
        {
            if (candidates.Count == 0 || accumulator.Count == max)
            {
                yield return accumulator;
            }

            if (accumulator.Count == max)
            {
                yield break;
            }

            for (var i = 0; i < candidates.Count; i++)
            {
                foreach (var value in PermuteInner(accumulator.Append(candidates[i]).ToList(), candidates.Omit(i), max))
                {
                    yield return value;
                }
            }
        }

        return PermuteInner([], values, max);
    }

    private static List<T> Omit<T>(this List<T> list, int index)
    {
        var copy = new List<T>(list);
        copy.RemoveAt(index);
        return copy;
    }
}
