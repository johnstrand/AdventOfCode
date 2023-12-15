var head = new Pos(5, 5);
var tail = new Pos(5, 5);

static int Normalize(int v) => v == 0 ? 0 : v / Math.Abs(v);

foreach (var length in new[] { 2, 10 })
{
    var tailHistory = new HashSet<(int x, int y)>();
    var rope = Enumerable.Range(0, length).Select(_ => new Pos(15, 15)).ToList();

    foreach (var instr in File.ReadAllLines("input.txt"))
    {
        var dir = instr[0];
        var len = int.Parse(instr[1..].Trim());
        for (var i = 0; i < len; i++)
        {
            // Head of the rope always moves
            rope[0] = new(
                rope[0].X + (dir == 'R' ? 1 : dir == 'L' ? -1 : 0),
                rope[0].Y + (dir == 'U' ? -1 : dir == 'D' ? 1 : 0));

            // Check if any of the following segments must move, going front to back
            for (var j = 1; j < rope.Count; j++)
            {
                var h = rope[j - 1];
                var t = rope[j];

                var dx = h.X - t.X;
                var dy = h.Y - t.Y;

                // Movement is only necessary if any of the |deltas| exceed 1
                if (Math.Abs(dx) > 1 || Math.Abs(dy) > 1)
                {
                    if (Math.Abs(dx) == Math.Abs(dy))
                    {
                        rope[j] = new(t.X + dx - Normalize(dx), t.Y + dy - Normalize(dy));
                    }
                    else if (Math.Abs(dx) > Math.Abs(dy))
                    {
                        rope[j] = new(t.X + dx - Normalize(dx), t.Y + dy);
                    }
                    else
                    {
                        rope[j] = new(t.X + dx, t.Y + dy - Normalize(dy));
                    }
                }
            }

            tailHistory.Add((rope[^1].X, rope[^1].Y));
        }
    }

    Console.WriteLine($"Length {length}: {tailHistory.Count}");
}

internal struct Pos(int x, int y)
{
    public int X = x;
    public int Y = y;

    public override readonly string ToString()
    {
        return $"{X}:{Y}";
    }
}