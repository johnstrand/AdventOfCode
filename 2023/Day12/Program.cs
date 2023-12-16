using AoC.Common;

static IEnumerable<char[]> ProduceMasks(string mask)
{
    var variants = 1L << mask.Count(m => m == '?');
    var arr = mask.ToCharArray();
    var indices = arr.IndexesOf('?').ToList();
    var bits = Enumerable.Range(0, indices.Count).Select(n => 1 << n).ToList();

    for (var i = 0L; i < variants; i++)
    {
        for (var j = 0; j < indices.Count; j++)
        {
            var v = (i & bits[j]) != 0;
            arr[indices[j]] = v ? '.' : '#';
        }
        yield return [.. arr];
    }
}

static List<int> Collapse(char[] arr)
{
    var groups = new string(arr).Split('.', StringSplitOptions.RemoveEmptyEntries);
    return groups.Select(g => g.Length).ToList();
}

var part1 = 0;

foreach (var line in File.ReadAllLines("input-test.txt"))
{
    var (mask, pattern) = line.ToTuple(' ');
    // mask = string.Join("?", Enumerable.Repeat(mask, 5));

    var expectedPattern = pattern.ToNumbers32(',').ToList();

    // expectedPattern = Enumerable.Repeat(expectedPattern, 5).SelectMany(m => m.ToList()).ToList();

    var combinations = (int)Math.Pow(2, mask.Count(m => m == '?'));
    var val = 0;
    var progress = 0;
    var masks = ProduceMasks(mask);
    foreach (var m in masks)
    {
        progress++;
        var collapsedMask = Collapse(m);

        Console.Write($"{mask} >> {val} [{progress:N0} / {combinations:N0}]\r");

        if (expectedPattern.Count != collapsedMask.Count)
        {
            continue;
        }

        if (expectedPattern.SequenceEqual(collapsedMask))
        {
            val++;
        }
    }
    Console.WriteLine($"{mask} >> {val}");
    part1 += val;
}

Render.Result("Part 1", part1);
