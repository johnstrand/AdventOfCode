
using System.Text.RegularExpressions;

using AoC.Common;

var valves = new Dictionary<string, Valve>();

var router = new Dijkstra<Valve>();

foreach (var line in File.ReadAllLines("input-test.txt"))
{
    var parsed = Regex.Match(line, @"Valve (.+?) has flow rate=(\d+); tunnels? leads? to valves? (.+)");
    var valve = new Valve(parsed.Groups[1].Value, int.Parse(parsed.Groups[2].Value), parsed.Groups[3].Value.Split(", ").ToList());
    router.AddNode(valves[valve.Id] = valve);
}

foreach (var node in valves.Values)
{
    foreach (var edge in node.Connections.Select(id => valves[id]))
    {
        router.AddEdge(node, edge);
    }
}
var path = router.Solve(valves["JJ"], valves["HH"]);
Console.WriteLine();

internal record Valve(string Id, int Flow, List<string> Connections);