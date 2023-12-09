namespace AoC.Common;
public class ObjWriter
{
    private static readonly List<Point3> vertOffsets =
    [
        new(0, 0,  0),
        new(0, 0,  1),
        new(0, 1,  0),
        new(0, 1,  1),
        new(1, 0,  0),
        new(1, 0,  1),
        new(1, 1,  0),
        new(1, 1,  1)
    ];

    private static readonly List<Point3> triTemplates =
    [
        new(1, 7, 5),
        new(1, 3, 7),
        new(1, 4, 3),
        new(1, 2, 4),
        new(3, 8, 7),
        new(3, 4, 8),
        new(5, 7, 8),
        new(5, 8, 6),
        new(1, 5, 6),
        new(1, 6, 2),
        new(2, 6, 8),
        new(2, 8, 4)
    ];

    private readonly List<List<int>> _tris = [];
    private readonly List<Point3> _verts = [];

    public void AddCube(Point3 pos, int size = 1)
    {
        var faceOffset = _verts.Count;

        _verts.AddRange(vertOffsets.Select(n => (n * size) + pos));

        foreach (var t in triTemplates)
        {
            _tris.Add(
            [
                faceOffset + t.X,
                faceOffset + t.Y,
                faceOffset + t.Z,
            ]);
        }
    }

    public void WriteTo(Stream target)
    {
        using var writer = new StreamWriter(target);
        writer.WriteLine("# Advent of code");
        writer.WriteLine();
        writer.WriteLine("g cube");
        writer.WriteLine();
        foreach (var vert in _verts)
        {
            writer.WriteLine($"v {vert.X} {vert.Y} {vert.Z}");
        }
        writer.WriteLine();
        foreach (var f in _tris)
        {
            writer.WriteLine($"f {string.Join(" ", f)}");
        }
    }
}
