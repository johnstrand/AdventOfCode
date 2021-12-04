var input = new Queue<string>(File.ReadAllText("Input.txt").Split(' '));
var tree = ReadNode(input);
Console.WriteLine($"Metadata sum: {tree.SimpleValue}");
Console.WriteLine($"Complex metadata sum: {tree.ComplexValue}");
Console.Read();

Node ReadNode(Queue<string> data)
{
    var childNodeCount = int.Parse(data.Dequeue());
    var metaDataCount = int.Parse(data.Dequeue());
    var children = Enumerable.Range(1, childNodeCount).Select(_ => ReadNode(data)).ToList();

    return new Node
    {
        Children = children,
        Metadata = Enumerable.Range(1, metaDataCount).Select(_ => int.Parse(data.Dequeue())).ToList()
    };
}

internal class Node
{
    public List<int> Metadata { get; set; }
    public List<Node> Children { get; set; }
    public int SimpleValue => Metadata.Sum() + Children.Select(c => c.SimpleValue).Sum();
    public int ComplexValue => Children.Count > 0 ? Metadata.Where(v => v <= Children.Count).Select(v => Children[v - 1].ComplexValue).Sum() : SimpleValue;
}
