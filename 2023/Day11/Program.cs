using AoC.Common;

HashSet<long> expansionColumns = null!;
var expansionRows = new List<long>();
var galaxies = new List<(long x, long y)>();

Console.WriteLine("Reading data");
var y = 0;
foreach (var line in File.ReadLines("input.txt"))
{
    if (expansionColumns == null)
    {
        expansionColumns = Enumerable.Range(0, line.Length).Where(ix => line[ix] == '.').As<int, long>().ToHashSet();
    }
    else
    {
        foreach (var index in Enumerable.Range(0, line.Length).Where(ix => line[ix] != '.'))
        {
            expansionColumns.Remove(index);
        }
    }

    for (var x = 0; x < line.Length; x++)
    {
        if (line[x] == '#')
        {
            galaxies.Add((x, y));
        }
    }

    if (line.All(c => c == '.'))
    {
        expansionRows.Add(y);
    }
    y++;
}

Console.WriteLine("Calculating distances");
var part1 = 0L;
var part2 = 0L;

(long, long) XDist(long x1, long x2)
{
    if (x1 > x2)
    {
        (x1, x2) = (x2, x1);
    }

    var expansions = expansionColumns.Count(c => c >= x1 && c <= x2);

    return (x2 - x1 + expansions, x2 - x1 + (expansions * 999_999));
}

(long, long) YDist(long y1, long y2)
{
    if (y1 > y2)
    {
        (y1, y2) = (y2, y1);
    }

    var expansions = expansionRows!.Count(r => r >= y1 && r <= y2);

    return (y2 - y1 + expansions, y2 - y1 + (expansions * 999_999));
}

for (var i = 0; i < galaxies.Count - 1; i++)
{
    for (var j = i + 1; j < galaxies.Count; j++)
    {
        var (x1, x2) = XDist(galaxies[j].x, galaxies[i].x);

        var (y1, y2) = YDist(galaxies[j].y, galaxies[i].y);
        part1 += x1 + y1;
        part2 += x2 + y2;
    }
}

Render.Result("Part 1", part1);
Render.Result("Part 2", part2);
