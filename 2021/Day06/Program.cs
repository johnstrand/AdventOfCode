using System.Collections.Concurrent;

var fishes = File.ReadAllText("input.txt").Split(',').Select(int.Parse).ToList();

ConcurrentDictionary<int, long> cache = new();

long GetCount(int start, int age, int max)
{
    var firstSpawn = start + age + 1;
    return cache.GetOrAdd(firstSpawn, _ =>
    {
        var sum = 1L;

        for (var nextSpawn = firstSpawn; nextSpawn <= max; nextSpawn += 7)
        {
            sum += GetCount(nextSpawn, 8, max);
        }

        return sum;
    });
}


foreach (var limit in new[] { 80, 256 })
{
    cache.Clear();
    var count = fishes.Select(n => GetCount(0, n, limit)).Sum();
    Console.WriteLine($"Limit: {limit} = {count}");
}
