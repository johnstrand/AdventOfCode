var routes = new Dictionary<string, int>();
var max = int.MinValue;
var min = int.MaxValue;

var places = new HashSet<string>();
foreach (var line in File.ReadAllLines("input.txt"))
{
    var parts = line.Split(' ');
    places.Add(parts[0]);
    places.Add(parts[2]);
    routes.Add($"{parts[0]}_{parts[2]}", int.Parse(parts[4]));
    routes.Add($"{parts[2]}_{parts[0]}", int.Parse(parts[4]));
}
Parallel.ForEach(places, item =>
{
    Console.WriteLine($"Starting from {item}");
    Travel(item, places.Where(p => p != item).ToList(), 0);
});

Console.WriteLine($"Min {min}");
Console.WriteLine($"Max {max}");

void Travel(string from, IEnumerable<string> remaining, int distance)
{
    if (!remaining.Any())
    {
        lock (routes)
        {
            max = Math.Max(distance, max);
            min = Math.Min(distance, min);
        }
    }
    foreach (var to in remaining)
    {
        Travel(to, remaining.Where(r => r != to).ToList(), distance + routes[$"{from}_{to}"]);
    }
}