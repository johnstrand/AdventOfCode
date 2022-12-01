﻿var links = File.ReadAllLines("input.txt").Select(row => row.Split('-')).Select(row => (a: row[0], b: row[1])).ToList();
var smallRooms = links.SelectMany(ln => new[] { ln.a, ln.b }).Where(n => n != "start" && n != "end" && n.ToLower() == n).ToList();

var paths = new HashSet<string>();
Step("start", new List<string>());
Console.WriteLine($"Part 1: {paths.Count}");
paths.Clear();
foreach (var link in smallRooms)
{
    Step("start", new List<string>(), link);
}
Console.WriteLine($"Part 2: {paths.Count}");

void Step(string step, List<string> history, string? twice = null)
{
    if (step == "end")
    {
        var path = string.Join(", ", history.Append(step));
        paths.Add(path);
        return;
    }

    var next = links!.Where(link => (link.a == step && link.b != "start") || (link.b == step && link.a != "start")).Select(link => link.a == step ? link.b : link.a).ToList();
    foreach (var n in next)
    {
        if (!CanVisit(n, history, twice))
        {
            continue;
        }

        Step(n, history.Append(step).ToList(), twice);
    }
}

bool CanVisit(string next, List<string> history, string? extra = null)
{
    if (next != next.ToLower())
    {
        return true;
    }

    if (extra == null || next != extra)
    {
        return !history.Contains(next);
    }

    return history.Count(n => n == next) < 2;
}