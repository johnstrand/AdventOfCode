using System.Text.RegularExpressions;

// TODO: Refactor all of this
var buckets = new List<List<int>>();
foreach (var item in File.ReadAllLines("input.txt"))
{
    var match = Regex.Match(item, @"(\w+): capacity (-?\d+), durability (-?\d+), flavor (-?\d+), texture (-?\d+), calories (-?\d+)");
    buckets.Add(new List<int>());
    for (var group = 2; group < match.Groups.Count; group++)
    {
        buckets[^1].Add(int.Parse(match.Groups[group].Value));
    }
}
var part1 = 0;
var part2 = 0;
for (var b0 = 0; b0 <= 100; b0++)
{
    for (var b1 = 0; b1 <= 100 - b0; b1++)
    {
        for (var b2 = 0; b2 <= 100 - b0 - b1; b2++)
        {
            var b3 = 100 - b0 - b1 - b2;
            var product =
                Math.Max((buckets[0][0] * b0) + (buckets[1][0] * b1) + (buckets[2][0] * b2) + (buckets[3][0] * b3), 0) *
                Math.Max((buckets[0][1] * b0) + (buckets[1][1] * b1) + (buckets[2][1] * b2) + (buckets[3][1] * b3), 0) *
                Math.Max((buckets[0][2] * b0) + (buckets[1][2] * b1) + (buckets[2][2] * b2) + (buckets[3][2] * b3), 0) *
                Math.Max((buckets[0][3] * b0) + (buckets[1][3] * b1) + (buckets[2][3] * b2) + (buckets[3][3] * b3), 0);

            if (product > part1)
            {
                part1 = product;
            }

            if ((buckets[0][4] * b0) + (buckets[1][4] * b1) + (buckets[2][4] * b2) + (buckets[3][4] * b3) == 500 && product > part2)
            {
                part2 = product;
            }
        }
    }
}

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");