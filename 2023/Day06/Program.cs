using AoC.Common;

var input = File.ReadAllLines("input.txt");
var times = input[0].Split(':')[1].ToNumbers64().ToList();
var distances = input[1].Split(':')[1].ToNumbers64().ToList();

var part1 = 1L;
foreach (var (time, distance) in times.Zip(distances))
{
    var wins = 0;
    Console.WriteLine($"Time: {time}, Distance: {distance}");
    for (var i = 0; i < time; i++)
    {
        if (i * (time - i) > distance)
        {
            wins++;
        }
    }
    part1 *= wins;
}

Render.Result("Part 1", part1);

var time2 = long.Parse(string.Concat(times));
var dist2 = long.Parse(string.Concat(distances));

var part2 = 0L;
for (var i = 0; i < time2; i++)
{
    if (i * (time2 - i) > dist2)
    {
        part2++;
    }
    else if (part2 > 0)
    {
        break;
    }
}

Render.Result("Part 2", part2);
