
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

var variants = Fact(candidateValues.Count);
var maxP = 0;
var sync = new object();
var progress = 0L;

var flowMax = new Dictionary<string, int>();

foreach (var path in Permutations.Generate(candidateValues))
{
    progress++;
    Console.Write($"{progress:N0} / {variants:N0}\r");
    var currentNode = "AA";
    var totalTime = 0;
    var totalP = 0;

    //foreach (var node in path)
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
        if (flowMax.TryGetValue(node, out var m) && m > totalP)
        {
            break;
        }

        flowMax[node] = totalP;
        currentNode = node;
    }

    if (totalP > maxP)
    {
        maxP = totalP;
        Console.WriteLine();
        Console.WriteLine($"Total: {maxP}");
    }

}

Console.WriteLine();

static long Fact(long v)
{
    return v < 2 ? 1 : v * Fact(v - 1);
}

internal record Valve(string Id, int Flow, List<string> Connections, bool Open = false);