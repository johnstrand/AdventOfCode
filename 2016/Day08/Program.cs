using AoC.Common;

var test = false;

var w = test ? 7 : 50;
var h = test ? 3 : 6;
var source = test ? "input-test.txt" : "input.txt";

var display = Enumerable.Repeat(' ', w * h).ToList();

void Show()
{
    Console.Clear();
    for (var offset = 0; offset < display.Count; offset += w)
    {
        Console.WriteLine(new string([.. display.GetRange(offset, w)]));
    }
    Task.Delay(10).Wait();
}

foreach (var cmd in File.ReadAllLines(source))
{
    var commands = cmd.SplitRemoveEmpty(' ', '=', 'b', 'x', 'y');
    if (commands[0] == "rect")
    {
        for (var y = 0; y < int.Parse(commands[2]); y++)
        {
            for (var x = 0; x < int.Parse(commands[1]); x++)
            {
                display[(y * w) + x] = '#';
            }
        }
    }
    else if (commands[1] == "column")
    {
        var x = int.Parse(commands[2]);
        var distance = int.Parse(commands[^1]);
        var original = display.ToList();
        for (var y = 0; y < h; y++)
        {
            var sourceIndex = (y * w) + x;
            var targetIndex = ((y + distance) % h * w) + x;
            display[targetIndex] = original[sourceIndex];
        }
    }
    else
    {
        var y = int.Parse(commands[2]);
        var distance = int.Parse(commands[^1]);
        var original = display.ToList();
        for (var x = 0; x < w; x++)
        {
            var sourceIndex = (y * w) + x;
            var targetIndex = (y * w) + ((x + distance) % w);
            display[targetIndex] = original[sourceIndex];
        }
    }

    Show();
}

var part1 = display.Count(c => c == '#');

Render.Result("Part 1", part1);
