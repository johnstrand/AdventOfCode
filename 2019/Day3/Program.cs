using SFML.Graphics;
Dictionary<Point, int> nodeCount = [];

var wires = File.ReadAllLines("input.txt");

using var window = new RenderWindow(new SFML.Window.VideoMode(800, 600), "Advent of Code");
using var dotTexture = new Texture("dot.png");
using var dot = new Sprite(dotTexture);
dot.Scale = new SFML.System.Vector2f(10, 10);
var view = window.DefaultView;
view.Move(new SFML.System.Vector2f(-400, -300));
view.Zoom(3f);
window.SetView(view);
window.Closed += (s, e) => window.Close();

var t = Task.Run(() =>
{
    foreach (var wire in wires)
    {
        Eval(wire.Split(',').ToList(), new Point(0, 0), dot);
    }

    /*
    var minx = nodeCount.Min(n => n.Key.X);
    var miny = nodeCount.Min(n => n.Key.Y);
    var maxx = nodeCount.Max(n => n.Key.X);
    var maxy = nodeCount.Max(n => n.Key.Y);
    */
    var min = nodeCount.Keys.Where(n => nodeCount[n] > 1 && n != new Point(0, 0)).Min(n => Math.Abs(n.X) + Math.Abs(n.Y));
    Console.WriteLine(min);
});

while (window.IsOpen)
{
    //window.Clear();
    window.DispatchEvents();
    window.Draw(dot);
    window.Display();
}

await t;

void Eval(List<string> instr, Point pos, Sprite dot)
{
    if (instr.Count == 0)
    {
        return;
    }

    var cur = instr[0];
    var dir = cur[0];
    var dist = int.Parse(cur[1..]);

    var vec = new Point(
        dir == 'L' ? -1 : dir == 'R' ? 1 : 0,
        dir == 'D' ? 1 : dir == 'U' ? -1 : 0
        );

    for (var _ = 0; _ < dist; _++)
    {
        dot.Color = nodeCount.ContainsKey(pos) ? Color.Red : Color.White;
        dot.Position = new SFML.System.Vector2f(pos.X, pos.Y);
        if (nodeCount.TryGetValue(pos, out var value))
        {
            nodeCount[pos] = ++value;
            Console.WriteLine(pos);
            Console.WriteLine(Math.Abs(pos.X) + Math.Abs(pos.Y));
        }
        else
        {
            nodeCount.Add(pos, 1);
        }

        pos += vec;
    }
    Task.Delay(10).Wait();

    Eval(instr.Skip(1).ToList(), pos, dot);
}

internal struct Point
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X;
    public int Y;

    public static bool operator ==(Point p1, Point p2) => p1.X == p2.X && p1.Y == p2.Y;

    public static bool operator !=(Point p1, Point p2) => p1.X != p2.X || p1.Y != p2.Y;

    public static Point operator +(Point p1, Point p2) => new(p1.X + p2.X, p1.Y + p2.Y);

    public override bool Equals(object obj)
    {
        return obj != null && (obj is Point p) && p == this;
    }

    public override int GetHashCode()
    {
        return (X * Y).GetHashCode();
    }

    public override string ToString()
    {
        return $" {X} , {Y} ";
    }
}