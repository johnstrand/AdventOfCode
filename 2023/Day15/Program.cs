using AoC.Common;

static int Hash(string str)
{
    var value = 0;
    foreach (var val in str)
    {
        value += val;
        value *= 17;
        value %= 256;
    }
    return value;
}

var part1 = File.ReadLines("input.txt").SelectMany(line => line.Split(',')).Select(p => (long)Hash(p)).Sum();

Render.Result("Part 1", part1);

var boxes = Enumerable.Range(0, 256).Select(_ => new List<string>()).ToArray();

foreach (var instr in File.ReadLines("input.txt").SelectMany(line => line.Split(',')))
{
    var _instr = instr;
    var remove = false;
    var value = "";

    if (instr[^1] == '-')
    {
        _instr = instr[0..^1];
        remove = true;
    }
    else if (instr.Contains('='))
    {
        var parts = instr.Split('=');
        _instr = parts[0];
        value = parts[1];
    }

    var boxId = Hash(_instr);

    if (remove)
    {
        var toRemove = boxes[boxId].Find(val => val.StartsWith(_instr));
        if (toRemove != null)
        {
            boxes[boxId].Remove(toRemove);
        }
    }
    else
    {
        var currentIndex = boxes[boxId].FindIndex(val => val.StartsWith(_instr));

        if (currentIndex != -1)
        {
            boxes[boxId][currentIndex] = $"{_instr} {value}";
        }
        else
        {
            boxes[boxId].Add($"{_instr} {value}");
        }
    }
}

var part2 = 0;
for (var i = 0; i < 256; i++)
{
    if (boxes[i].Count > 0)
    {
        for (var j = 0; j < boxes[i].Count; j++)
        {
            var focalLen = int.Parse(boxes[i][j].Split(' ')[1]);
            var lensValue = (i + 1) * (j + 1) * focalLen;
            part2 += lensValue;
        }
    }
}

Render.Result("Part 2", part2);
