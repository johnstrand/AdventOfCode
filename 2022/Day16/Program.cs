
using System.Text.RegularExpressions;

using AoC.Common;

var valves = new Dictionary<string, Valve>();

var router = new Dijkstra<string>();

foreach (var line in File.ReadAllLines("input.txt"))
{
    var parsed = Regex.Match(line, @"Valve (.+?) has flow rate=(\d+); tunnels? leads? to valves? (.+)");
    var valve = new Valve(parsed.Groups[1].Value, int.Parse(parsed.Groups[2].Value), parsed.Groups[3].Value.Split(", ").ToList());
    // Uh, so, right
    router.AddNode((valves[valve.Id] = valve).Id);
}

foreach (var node in valves.Values)
{
    foreach (var edge in node.Connections.Select(id => valves[id]))
    {
        router.AddEdge(node.Id, edge.Id);
    }
}

var candidateValues = valves.Values.Where(v => v.Flow > 0).Select(v => v.Id).ToList();

var paths = new Queue<List<string>>(candidateValues.Select(c => new List<string> { c }));

var maxP = 0;

while (paths.Count > 0)
{
    Console.Write($"Max: {maxP}. Pending: {paths.Count:N0}     \r");
    var path = paths.Dequeue();
    var currentNode = "AA";
    var totalTime = 0;
    var totalP = 0;

    for (var i = 0; i < path.Count; i++)
    {
        var node = path[i];
        var route = router.Solve(currentNode, node);
        var deltaTime = route.Count;
        totalTime += deltaTime;
        if (totalTime > 30)
        {
            break;
        }

        totalP += (30 - totalTime) * valves[node].Flow;
        currentNode = node;
    }

    if (totalTime < 30)
    {
        foreach (var remaining in candidateValues.Where(c => !path.Contains(c)))
        {
            paths.Enqueue(path.Append(remaining).ToList());
        }
        continue;
    }

    if (totalP > maxP)
    {
        maxP = totalP;
    }
}

Console.Write($"Max: {maxP}. Pending: {paths.Count:N0}     \r");
Console.WriteLine();

internal record Valve(string Id, int Flow, List<string> Connections, bool Open = false);