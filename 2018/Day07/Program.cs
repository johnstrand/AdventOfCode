// TODO: This does not appear to be complete
using System.Text.RegularExpressions;

var pattern = "Step (?<resolver>.) must be finished before step (?<target>.) can begin.";
var steps = new List<Step>();
foreach (var row in File.ReadAllLines("input.txt"))
{
    var m = Regex.Match(row, pattern);
    var resolver = m.Groups["resolver"].Value;
    var target = m.Groups["target"].Value;
    if (!steps.Any(t => t.Name == resolver))
    {
        steps.Add(new Step { Name = resolver });
    }
    Step s;
    if ((s = steps.Find(t => t.Name == target)) == null)
    {
        steps.Add(s = new Step { Name = target });
    }
    s.ResolvedBy.Add(resolver);
}

while (steps.Any(s => !s.Resolved))
{
    var next = steps
        .Where(s => s.ResolvedBy.Count == 0 && !s.Resolved)
        .OrderBy(s => s.Name).First();

    Console.Write(next.Name);
    next.Resolved = true;
    foreach (var step in steps)
    {
        step.ResolvedBy.Remove(next.Name);
    }
}

internal class Step
{
    public string Name { get; set; }
    public HashSet<string> ResolvedBy { get; set; } = [];
    public bool Resolved { get; set; }
}