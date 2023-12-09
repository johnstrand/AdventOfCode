var raindeer = new List<Raindeer>();

foreach (var row in File.ReadAllLines("input.txt").Select(r => r.Split(' ')))
{
    // 0 3 6 13
    var name = row[0];
    var speed = int.Parse(row[3]);
    var duration = int.Parse(row[6]);
    var rest = int.Parse(row[13]);
    raindeer.Add(new Raindeer(name, speed, duration, rest));
}

for (var tick = 0; tick < 2504; tick++)
{
    raindeer.ForEach(r => r.Update());
    var lead = raindeer.First(r => r.Distance == raindeer.Max(rd => rd.Distance));
    lead.Points++;
}

Console.WriteLine($"Part 1: {raindeer.Max(r => r.Distance)}");
Console.WriteLine($"Part 2: {raindeer.Max(r => r.Points)}");

internal class Raindeer(string name, int speed, int duration, int rest)
{
    private int restRemaining;
    private int durationRemaining = duration;

    public int Distance { get; private set; }
    public string Name { get; } = name;
    public int Points { get; set; }

    public void Update()
    {
        if (durationRemaining > 0)
        {
            durationRemaining--;
            Distance += speed;
        }
        else if (durationRemaining == 0 && restRemaining == 0)
        {
            // Already rested a tick
            restRemaining = rest - 1;
            durationRemaining = -1;
        }
        else if (restRemaining > 0)
        {
            restRemaining--;
        }
        else if (restRemaining == 0)
        {
            durationRemaining = duration - 1;
            Distance += speed;
        }
    }

    public override string ToString()
    {
        return $"{Name} - {Distance}";
    }
}