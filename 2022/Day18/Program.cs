using AoC.Common;

var nodes = new HashSet<Point3>();



foreach (var row in File.ReadAllLines("input.txt"))
{
    var coords = row.Split(",").Select(int.Parse).ToList();
    var node = new Point3(coords[0], coords[1], coords[2]);

    nodes.Add(node);

}

var visited = new HashSet<Point3>();

var deltas = new[]
{
    new Point3(x: 1, y: 0, z: 0),
    new(x: -1, y: 0, z: 0),
    new(x: 0, y: 1, z: 0),
    new(x: 0, y: -1, z: 0),
    new(x: 0, y: 0, z: 1),
    new(x: 0, y: 0, z: -1),
};

var faces = 0;

void Visit(Point3 node, int depth = 0)
{
    if (!visited!.Add(node))
    {
        return;
    }

    foreach (var adjacent in deltas.Select(d => d + node))
    {
        if (!nodes!.Contains(adjacent))
        {
            faces++;
        }
        else
        {
            Visit(adjacent, depth + 1);
        }
    }
}

while (visited.Count != nodes.Count)
{
    Visit(nodes.First(n => !visited.Contains(n)));
}

Console.WriteLine($"Part 1: {faces}");

var minX = nodes.Min(n => n.X);
var minY = nodes.Min(n => n.Y);
var minZ = nodes.Min(n => n.Z);

var maxX = nodes.Max(n => n.X);
var maxY = nodes.Max(n => n.Y);
var maxZ = nodes.Max(n => n.Z);

for (var z = minZ; z <= maxZ; z++)
{
    for (var y = minY; y <= maxY; y++)
    {
        for (var x = minX; x <= maxX; x++)
        {
            var candidate = new Point3(x, y, z);

            if (nodes.Contains(candidate))
            {
                continue;
            }

            var temp = new HashSet<Point3>() { candidate };
            var pending = new Queue<Point3>();

            pending.Enqueue(candidate);

            while (pending.Count > 0)
            {
                var point = pending.Dequeue();

                if (nodes.Contains(point))
                {
                    continue;
                }

                foreach (var adjacent in deltas.Select(d => d + point))
                {
                    if (nodes.Contains(adjacent) || !temp.Add(adjacent))
                    {
                        continue;
                    }

                    if (adjacent.X < minX || adjacent.Y < minY || adjacent.Z < minZ || adjacent.X > maxX || adjacent.Y > maxY || adjacent.Z > maxZ)
                    {
                        temp.Clear();
                        break;
                    }

                    pending.Enqueue(adjacent);
                }

                if (temp.Count == 0)
                {
                    break;
                }
            }

            foreach (var n in temp)
            {
                nodes.Add(n);
            }
        }
    }
}

visited.Clear();
faces = 0;

while (visited.Count != nodes.Count)
{
    Visit(nodes.First(n => !visited.Contains(n)));
}

Console.WriteLine($"Part 2: {faces}");
