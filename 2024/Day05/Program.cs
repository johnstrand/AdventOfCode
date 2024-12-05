using AoC.Common;

var rules = new List<(int first, int follows)>();
var rulePart = true;

var input = Input.ReadActual();
int i;

for (i = 0; i < input.Count; i++)
{
    var row = input[i];
    if (string.IsNullOrWhiteSpace(row))
    {
        i++;
        break;
    }

    if (rulePart)
    {
        var parts = row.ToNumbers32('|').ToArray();

        rules.Add((parts[0], parts[1]));
    }
}

var part1 = 0;

for (; i < input.Count; i++)
{
    var row = input[i].ToNumbers32(',').ToList();

    var lookup = row
        .Select((value, index) => (value, index))
        .ToDictionary(v => v.value, v => v.index);

    var valid = true;

    for (var j = 0; j < row.Count; j++)
    {
        var current = row[j];

        var matchingRules = rules
            .Where(r => r.first == current && lookup.ContainsKey(r.follows))
            .Select(r => r.follows)
            .ToList();

        if (matchingRules.Any(r => lookup[r] < j))
        {
            valid = false;
            break;
        }
    }

    if (valid)
    {
        part1 += row[row.Count / 2];
        continue;
    }
}

Console.WriteLine($"Part 1: {part1}");
