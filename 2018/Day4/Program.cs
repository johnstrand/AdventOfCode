using System.Globalization;
using System.Text.RegularExpressions;

var eventstream = File.ReadAllLines("Input.txt")
    .Select(ParseEvent)
    .OrderBy(e => e.Timestamp).ToList();

var lastSeen = -1;
var beginSleep = DateTime.MinValue;
var sleepTracker = new Dictionary<int, int>();
var sleepRanges = new Dictionary<int, List<(DateTime start, DateTime end)>>();

foreach (var ev in eventstream)
{
    if (ev.Type == "BEGIN")
    {
        lastSeen = ev.Id;
        if (!sleepTracker.ContainsKey(lastSeen))
        {
            sleepTracker.Add(lastSeen, 0);
            sleepRanges.Add(lastSeen, new List<(DateTime start, DateTime end)>());
        }
    }
    else
    {
        ev.Id = lastSeen;
    }

    if (ev.Type == "SLEEP")
    {
        beginSleep = ev.Timestamp;
    }

    if (ev.Type == "WAKE")
    {
        sleepTracker[lastSeen] += (int)(ev.Timestamp - beginSleep).TotalMinutes;
        sleepRanges[lastSeen].Add((beginSleep, ev.Timestamp));
    }
}

var mostAsleep = sleepTracker.OrderByDescending(kv => kv.Value).First();
var ranges = sleepRanges[mostAsleep.Key];

var mostCommonMinute = ranges
    .SelectMany(range => ReduceRange(range.start, range.end))
    .GroupBy(x => x)
    .Select(g => new { minute = g.Key, count = g.Count() })
    .OrderByDescending(x => x.count)
    .ToList();

var allRanges = sleepRanges.Select(range => new
{
    guard = range.Key,
    common = range.Value
        .SelectMany(v => ReduceRange(v.start, v.end))
        .GroupBy(x => x)
        .Select(g => new
        {
            minute = g.Key,
            count = g.Count()
        })
        .OrderByDescending(x => x.count)
        .FirstOrDefault() ?? new { minute = -1, count = 0 }
}).ToList();

Console.WriteLine($"ID: {mostAsleep.Key}. Minute: {mostCommonMinute[0].minute}. Checksum: {mostAsleep.Key * mostCommonMinute[0].minute}");

var mostFrequent = allRanges.OrderByDescending(r => r.common.count).First();

Console.WriteLine($"ID: {mostFrequent.guard}. Minute: {mostFrequent.common.minute}. Checksum: {mostFrequent.guard * mostFrequent.common.minute}");

Console.Read();

IEnumerable<int> ReduceRange(DateTime start, DateTime end)
{
    while (start < end)
    {
        yield return start.Minute;
        start = start.AddMinutes(1);
    }
}

Event ParseEvent(string text)
{
    var match = Regex.Match(text, @"\[(?<date>.+)\] (?<event>.+)");
    var ev = match.Groups["event"].Value;
    var id = "-1";
    if (ev.Contains('#'))
    {
        id = ev.Split(' ').Where(e => e.StartsWith("#")).Select(e => e[1..]).First();
    }
    return new Event
    {
        Timestamp = DateTime.ParseExact(match.Groups["date"].Value, "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture),
        Id = int.Parse(id),
        Type = ev.Contains("wakes") ? "WAKE" : ev.Contains("asleep") ? "SLEEP" : "BEGIN"
    };
}

internal class Event
{
    public int Id { get; set; }
    public string Type { get; set; }
    public DateTime Timestamp { get; set; }
    public override string ToString()
    {
        return $"[{Id}/{Timestamp}]: {Type}";
    }
}
