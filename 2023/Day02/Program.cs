using AoC.Common;

// These are the limits for each color
var limits = new Dictionary<string, int>
{
    ["red"] = 12,
    ["green"] = 13,
    ["blue"] = 14
};

var part1 = 0;
var part2 = 0;
foreach (var row in File.ReadAllLines("input.txt"))
{
    // Split the row into parts based on the fillers
    var parts = row.SplitRemoveEmpty(',', ';', ':', ' ');

    // Get the ID (the first element will just be "Game")
    var id = int.Parse(parts[1]);

    // Set the value to the ID
    var value1 = id;

    // This is how we keep track of the minimum requirements for each color
    var minimums = new Dictionary<string, int>
    {
        ["red"] = 0,
        ["green"] = 0,
        ["blue"] = 0
    };

    // What we have left is a list of value/color pairs
    for (var i = 2; i < parts.Length; i += 2)
    {
        var value = int.Parse(parts[i]);
        var color = parts[i + 1];

        // If the value is greater than the minimum, update the minimum
        if (value > minimums[color])
        {
            minimums[color] = value;
        }

        // If the value is greater than the limit, set the value of this game to 0
        if (value > limits[color])
        {
            value1 = 0;
        }
    }

    // Add the value to the total for part 1 (either ID or 0)
    part1 += value1;

    // Multiply the minimums together and add to the total for part 2
    part2 += minimums.Values.Aggregate(1, (a, b) => a * b);
}

Console.WriteLine(part1);
Console.WriteLine(part2);
