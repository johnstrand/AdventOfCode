var root = new Directory("/", null);

Directory? currentDirectory = null;
var commands = new Queue<string>(File.ReadAllLines("input.txt"));

while (commands.Count > 0)
{
    var cmd = commands.Dequeue();
    if (cmd[0] != '$')
    {
        throw new($"Input '{cmd}' was unexpected at this time");
    }

    var argc = cmd.Split(' ').Skip(1).ToArray();

    if (argc[0] == "cd")
    {
        if (argc[1] == "/")
        {
            currentDirectory = root;
        }
        else if (argc[1] == "..")
        {
            currentDirectory = currentDirectory?.Parent;
        }
        else
        {
            currentDirectory = currentDirectory?.Children[argc[1]];
        }
    }
    else if (argc[0] == "ls")
    {
        while (commands.Count > 0 && commands.Peek()[0] != '$')
        {
            var entry = commands.Dequeue().Split(' ');
            if (entry[0] == "dir")
            {
                currentDirectory?.AddChild(entry[1]);
            }
            else
            {
                currentDirectory?.AddFile(entry[1], long.Parse(entry[0]));
            }
        }
    }
    else
    {
        throw new($"Command '{argc[0]}' is not a known command");
    }
}

var part1 = root.Find(d => d.Size <= 100000).Sum(d => d.Size);

var spaceAvailable = 70_000_000 - root.Size;
var spaceNeeded = 30_000_000 - spaceAvailable;

var part2 = root.Find(d => d.Size >= spaceNeeded).OrderBy(d => d.Size).First().Size;

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

internal class Directory
{
    public string Path { get; }

    public long Size => Files.Values.Sum() + Children.Values.Sum(c => c.Size);

    public Directory? Parent { get; }

    public Dictionary<string, Directory> Children { get; } = [];

    public Dictionary<string, long> Files { get; } = [];

    public Directory(string path, Directory? parent)
    {
        Path = path;
        Parent = parent;
    }

    public void AddChild(string name)
    {
        Children[name] = new($"{Path}{name}/", this);
    }

    public void AddFile(string file, long size)
    {
        Files[file] = size;
    }

    public IEnumerable<Directory> Find(Func<Directory, bool> predicate)
    {
        if (predicate(this))
        {
            yield return this;
        }

        foreach (var directory in Children.Values.SelectMany(c => c.Find(predicate)))
        {
            yield return directory;
        }
    }
}
