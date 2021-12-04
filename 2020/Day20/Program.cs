// See https://aka.ms/new-console-template for more information
using var reader = new StreamReader("test.txt");

var fields = new List<Field>();
while (!reader.EndOfStream)
{
    var title = reader.ReadLine();
    if (string.IsNullOrEmpty(title))
    {
        continue;
    }
    var data = new List<string>();
    while (!reader.EndOfStream)
    {
        var row = reader.ReadLine();
        if (string.IsNullOrEmpty(row))
        {
            break;
        }
        data.Add(row);
    }
    fields.Add(new Field(title, data));
}

internal class Field
{
    private readonly string name;
    private readonly List<string> data;
    private readonly int width;
    private readonly List<int> matrix;

    public Field(string name, List<string> data)
    {
        this.name = name;
        this.data = data;
        width = data[0].Length - 1;
        matrix = CreateRotationMatrix(width * width);
    }

    public IEnumerable<string> Edges()
    {
        yield return data[0];
        yield return Reverse(data[0]);
        yield return data[^1];
        yield return Reverse(data[^1]);

        var leftEdge = new string(data.Select(r => r[0]).ToArray());
        var rightEdge = new string(data.Select(r => r[width]).ToArray());

        yield return leftEdge;
        yield return Reverse(leftEdge);
        yield return rightEdge;
        yield return Reverse(rightEdge);
    }

    private static string Reverse(string data)
    {
        return new string(data.Reverse().ToArray());
    }
    private static List<int> CreateRotationMatrix(int size)
    {
        var root = (int)Math.Sqrt(size);
        var matrix = new List<int>();
        for (var start = size - root; start < size; start++)
        {
            for (var index = start; index >= 0; index -= root)
            {
                matrix.Add(index);
            }
        }

        return matrix;
    }
}
