using System.Text.RegularExpressions;

var fabric = new int[1000 * 1000];
var idIndex = new Dictionary<int, List<int>>();
foreach (var row in File.ReadAllLines("Input.txt"))
{
    var m = Regex.Match(row, @"#(?<id>\d+) @ (?<start>\d+,\d+): (?<end>\d+x\d+)");
    var id = int.Parse(m.Groups["id"].Value);
    var start = Parse(m.Groups["start"].Value);
    var size = Parse(m.Groups["end"].Value);

    idIndex.Add(id, []);

    for (var y = start.Y; y < start.Y + size.Y; y++)
    {
        for (var x = start.X; x < start.X + size.X; x++)
        {
            fabric[x + (y * 1000)]++;
            idIndex[id].Add(x + (y * 1000));
        }
    }
}
Console.WriteLine(fabric.Count(i => i >= 2));

var intact = idIndex.FirstOrDefault(ix => ix.Value.All(i => fabric[i] == 1));

Console.WriteLine(intact.Key);

Console.Read();

static Point Parse(string text)
{
    var parts = text.Split(',', 'x').Select(int.Parse).ToList();

    return new Point(parts[0], parts[1]);
}

internal class Point(int x, int y)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
}
