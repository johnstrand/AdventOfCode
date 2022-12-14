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
    var matrix = Field.CreateRotationMatrix(data.Sum(s => s.Length));
    for (var i = 0; i < 4; i++)
    {
        fields.Add(new Field(title, data).Dump());
        Console.WriteLine();
        data = Field.Rotate(data, matrix);
    }
    data = Field.Flip(data);
    for (var i = 0; i < 4; i++)
    {
        fields.Add(new Field(title, data).Dump());
        Console.WriteLine();
        data = Field.Rotate(data, matrix);
    }
}

var gridSize = (int)Math.Sqrt(fields.Count / 8);

void Place(int x, int y, List<string> placed)
{
}

internal class Field
{
    private readonly string name;
    private readonly List<string> data;
    private readonly int width;

    public string Top => data[0];
    public string Bottom => data[^1];
    public string Left => new(data.Select(r => r[0]).ToArray());
    public string Right => new(data.Select(r => r[width - 1]).ToArray());

    public Field(string name, List<string> data)
    {
        this.name = name;
        this.data = data;
        width = data[0].Length;
    }

    public Field Dump()
    {
        Console.WriteLine(name);
        Console.WriteLine(string.Join(Environment.NewLine, data));
        return this;
    }

    private static string Reverse(string data)
    {
        return new string(data.Reverse().ToArray());
    }

    public static List<int> CreateRotationMatrix(int size)
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

    public static List<string> Flip(List<string> data)
    {
        return data.ConvertAll(Reverse);
    }

    public static List<string> Rotate(List<string> data, List<int> matrix)
    {
        var _t = data.SelectMany(r => r.ToArray()).ToList();
        var rotated = Enumerable.Repeat(' ', _t.Count).ToList();
        for (var index = 0; index < _t.Count; index++)
        {
            rotated[index] = _t[matrix[index]];
        }

        var size = data[0].Length;
        var final = new List<string>();
        for (var offset = 0; offset < rotated.Count; offset += size)
        {
            final.Add(new string(rotated.Skip(offset).Take(size).ToArray()));
        }
        return final;
    }
}
