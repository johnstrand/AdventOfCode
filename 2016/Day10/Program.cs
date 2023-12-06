using System.Text.RegularExpressions;

using AoC.Common;

Console.WriteLine("Reading input...");
var bots = new Dictionary<long, Bot>();
var outputs = new Dictionary<long, long>();

Match m;
foreach (var line in File.ReadAllLines("input.txt"))
{
    if ((m = Regex.Match(line, @"value (\d+) goes to bot (\d+)")).Success)
    {
        var value = long.Parse(m.Groups[1].Value);
        var id = long.Parse(m.Groups[2].Value);

        if (!bots.TryGetValue(id, out var bot))
        {
            bots[id] = new(id, 0, 0, []);
        }

        bots[id].Chips.Add(value);
    }
    else if ((m = Regex.Match(line, @"bot (\d+) gives low to (.+?) (\d+) and high to (.+?) (\d+)")).Success)
    {
        var id = long.Parse(m.Groups[1].Value);
        var lowType = m.Groups[2].Value;
        var lowId = long.Parse(m.Groups[3].Value);
        var highType = m.Groups[4].Value;
        var highId = long.Parse(m.Groups[5].Value);

        if (!bots.TryGetValue(id, out var bot))
        {
            bots[id] = new(id, lowType == "output" ? -(lowId + 1) : lowId, highType == "output" ? -(highId + 1) : highId, []);
        }
        else
        {
            bot.TargetLow = lowType == "output" ? -(lowId + 1) : lowId;
            bot.TargetHigh = highType == "output" ? -(highId + 1) : highId;
        }
    }
}

Console.WriteLine("Processing rules");

while (true)
{
    var activeBots = bots.Values.Where(b => b.Chips.Count > 1).ToList();

    if (activeBots.Count == 0)
    {
        break;
    }

    foreach (var bot in activeBots)
    {
        var lowFirst = bot.Chips[0] < bot.Chips[1];

        var (low, high) = lowFirst ? (bot.Chips[0], bot.Chips[1]) : (bot.Chips[1], bot.Chips[0]);

        if (low == 17 && high == 61)
        {
            Render.Result("Part 1", bot.Id);
        }

        bot.Chips.Clear();

        if (bot.TargetLow >= 0)
        {
            bots[bot.TargetLow].Chips.Add(low);
        }
        else
        {
            outputs[bot.TargetLow] = low;
        }

        if (bot.TargetHigh >= 0)
        {
            bots[bot.TargetHigh].Chips.Add(high);
        }
        else
        {
            outputs[bot.TargetHigh] = high;
        }
    }
}

Render.Result("Part 2", outputs[-1] * outputs[-2] * outputs[-3]);

internal class Bot(long id, long targetLow, long targetHigh, List<long> chips)
{
    public long Id { get; set; } = id;
    public long TargetLow { get; set; } = targetLow;
    public long TargetHigh { get; set; } = targetHigh;
    public List<long> Chips { get; } = chips;

    public override string ToString()
    {
        return $"{Id} -> {Chips.Count}";
    }
}