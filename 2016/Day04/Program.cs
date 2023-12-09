using System.Text.RegularExpressions;

var sum1 = 0;
var sectorId = 0;
foreach (var row in File.ReadAllLines("input.txt"))
{
    var (name, id, checksum) = ParseRow(row);

    if (VerifyName(name, new Queue<char>(checksum)))
    {
        sum1 += id;
        if (sectorId == 0 && DecodeName(name, id) == "northpoleobjectstorage")
        {
            sectorId = id;
        }
    }
}

Console.WriteLine($"Part 1: {sum1}");
Console.WriteLine($"Part 2: {sectorId}");

static string DecodeName(string name, int id)
{
    return new string(name.Select(c => (char)(((c - 'a' + id) % 26) + 'a')).ToArray());
}

static (string name, int id, string checksum) ParseRow(string row)
{
    var match = Regex.Match(row, @"^(.+?)(\d+)\[(.+?)\]$");
    return (
        match.Groups[1].Value.Replace("-", ""),
        int.Parse(match.Groups[2].Value),
        match.Groups[3].Value);
}

static bool VerifyName(string name, Queue<char> checksum)
{
    var groupedCount = name
        .GroupBy(c => c)
        .GroupBy(c => c.Count(), c => c.Key)
        .ToDictionary(g => g.Key, g => g.ToList());

    foreach (var k in groupedCount.Keys.OrderByDescending(x => x))
    {
        while (checksum.Count > 0 && groupedCount[k].Count != 0)
        {
            var next = checksum.Dequeue();
            if (!groupedCount[k].Remove(next))
            {
                return false;
            }
        }
    }
    return true;
}
