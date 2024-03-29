﻿var lookup = new Dictionary<string, Body>
{
    { "COM", new Body { Name = "COM", Depth = 0 } }
};

var q = new Queue<string[]>(File.ReadAllLines("input.txt").Select(r => r.Split(')')));
while (q.Count > 0)
{
    var row = q.Dequeue();
    if (!lookup.ContainsKey(row[0]))
    {
        q.Enqueue(row);
        continue;
    }
    var p = lookup[row[0]];
    var c = new Body { Name = row[1], Depth = p.Depth + 1, Parent = p };
    p.Children.Add(c);
    lookup.Add(c.Name, c);
}

Console.WriteLine(lookup.Sum(kv => kv.Value.Depth));

var me = lookup["YOU"];
var steps = 0;
while (!me.FindChildRef("SAN"))
{
    me = me.Parent;
    steps++;
}

while (me.Name != "SAN")
{
    me = me.Children.First(c => c.FindChildRef("SAN"));
    steps++;
}

Console.WriteLine(steps - 2);

internal class Body
{
    public string Name { get; set; }
    public int Depth { get; set; }
    public Body Parent { get; set; }
    public List<Body> Children { get; set; } = [];

    public bool FindChildRef(string name)
    {
        return Name == name || Children.Any(child => child.FindChildRef(name));
    }
}