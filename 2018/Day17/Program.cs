// TODO: Also not complete
using System.Drawing;

var points = GetPoints().ToList();
var offset = points.Min(p => p.X);

foreach (var point in points)
{
    Console.SetCursorPosition(point.X - offset, point.Y);
    Console.Write('#');
}
Console.SetCursorPosition(500 - offset, 0);
Console.Write("+");
Console.Read();

static IEnumerable<Point> GetPoints()
{
    foreach (var row in File.ReadAllLines("Input.txt"))
    {
        var parts = row.Split(',').ToDictionary(
            x => x.Split('=')[0].Trim(),
            x => x.Split('=').Last().Trim().Split("..").Select(int.Parse).ToList());

        for (var y = parts["y"].Min(); y <= parts["y"].Max(); y++)
        {
            for (var x = parts["x"].Min(); x <= parts["x"].Max(); x++)
            {
                yield return new Point(x, y);
            }
        }
    }
}
