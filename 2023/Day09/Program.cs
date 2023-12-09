using AoC.Common;

IEnumerable<List<int>> GetDeltas(List<int> numbers)
{
    while (!numbers.All(n => n == 0))
    {
        // We do yield return before calculating new deltas for two reasons:
        // * We want the original values in our list of deltas
        // * We DON'T want a list of zeroes in our list of deltas, they are of no use to us
        yield return numbers;
        // We'll skip the last number, to avoid trying to read past the end of the list
        // For each index, construct a new list by subtracting the value of the current index
        // from the value of the next index
        numbers = numbers[0..^1].Select((x, i) => numbers[i + 1] - x).ToList();
    }
}

int Step(List<List<int>> deltas, Index ix, Func<int, int, int> op)
{
    // Start at the bottom, this will contain the lowest level of deltas
    for (var i = deltas.Count - 1; i >= 0; i--)
    {
        // If we're at the bottom, deltas shouldn't increase at all, so we can just grab one from the list
        // Otherwise, grab the first (or last, depending on supplied ix) from the current level of deltas and from the level of deltas below
        // and apply the operation to them (either adding or subtracting)
        var value = i == deltas.Count - 1 ? deltas[i][ix] : op(deltas[i][ix], deltas[i + 1][ix]);

        // If the ix is set to 0 (i.e., from the start), we'll need to insert the new value at the start of the current level
        if (ix.Value == 0)
        {
            deltas[i].Insert(0, value);
        }
        // otherwise, we'll append it
        else
        {
            deltas[i].Add(value);
        }
    }

    return deltas[0][ix];
}

var part1 = 0;
var part2 = 0;
foreach (var line in File.ReadLines("input.txt").Select(line => line.ToNumbers32()))
{
    var deltas = GetDeltas(line.ToList()).ToList();

    // Part 1 - Look forward and add. Index set to last entry
    part1 += Step(deltas, ^1, (a, b) => a + b);

    // Part 2 - Look backward and substract. Index set to the first entry
    part2 += Step(deltas, 0, (a, b) => a - b);
}

Render.Result("Part 1", part1);
Render.Result("Part 2", part2);
