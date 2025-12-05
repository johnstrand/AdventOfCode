using AoC.Common;

var rangeChecks = new List<Func<long, bool>>();

var ranges = new List<(long min, long max)>();

void CompileCheck(long min, long max)
{
    rangeChecks.Add(v => v >= min && v <= max);
}

void Add(long min, long max)
{
    ranges.Add((min, max));

    var fix = false;

    do
    {
        fix = false;
        for (var i = 0; i < ranges.Count - 1; i++)
        {
            var (min1, max1) = ranges[i];
            for (var j = i + 1; j < ranges.Count; j++)
            {
                var (min2, max2) = ranges[j];

                if (min1.InRange(min2, max2) || max1.InRange(min2, max2) || min2.InRange(min1, max1) || max2.InRange(min1, max1))
                {
                    ranges[i] = (Math.Min(min1, min2), Math.Max(max1, max2));
                    ranges.RemoveAt(j);
#pragma warning disable S127 // "for" loop stop conditions should be invariant
                    j--;
#pragma warning restore S127 // "for" loop stop conditions should be invariant
                    fix = true;
                }
            }
        }
    } while (fix);
}


var lines = Input.ReadActual();

var i = 0;

for (; !string.IsNullOrWhiteSpace(lines[i]); i++)
{
    var (min, max) = lines[i].ToTupleInt64('-');

    CompileCheck(min, max);
    Add(min, max);
}

i++;

var part1 = 0;
for (; i < lines.Count; i++)
{
    var value = long.Parse(lines[i]);

    if (rangeChecks.Exists(r => r(value)))
    {
        part1++;
    }
}

var part2 = 0L;

foreach (var (min, max) in ranges)
{
    part2 += (max - min) + 1;
}

Console.WriteLine($"Part 1 = {part1}");
Console.WriteLine($"Part 2 = {part2}");
