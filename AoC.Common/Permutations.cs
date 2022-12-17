namespace AoC.Common;
public static class Permutations
{
    public static IEnumerable<List<T>> Generate<T>(List<T> values)
    {
        static IEnumerable<List<T>> PermuteInner(List<T> accumulator, List<T> candidates)
        {
            if (candidates.Count == 0)
            {
                yield return accumulator;
            }

            for (var i = 0; i < candidates.Count; i++)
            {
                foreach (var value in PermuteInner(accumulator.Append(candidates[i]).ToList(), candidates.Omit(i)))
                {
                    yield return value;
                }
            }
        }

        return PermuteInner(new List<T>(), values);
    }

    private static List<T> Omit<T>(this List<T> list, int index)
    {
        var copy = new List<T>(list);
        copy.RemoveAt(index);
        return copy;
    }
}
