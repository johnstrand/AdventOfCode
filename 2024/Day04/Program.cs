using AoC.Common;

var part1 = 0;

var input = Input.ReadActual();
var w = input[0].Length;
var h = input.Count;

char Get(int x, int y)
{
    return y >= 0 && y < h && x >= 0 && x < w ? input[y][x] : '\0';
}

var grid = string.Concat(input);

for (var y = 0; y < h; y++)
{
    for (var x = 0; x < w; x++)
    {
        var horz = $"{Get(x, y)}{Get(x + 1, y)}{Get(x + 2, y)}{Get(x + 3, y)}";

        if (horz is "XMAS" or "SAMX")
        {
            part1++;
        }

        var vert = $"{Get(x, y)}{Get(x, y + 1)}{Get(x, y + 2)}{Get(x, y + 3)}";

        if (vert is "XMAS" or "SAMX")
        {
            part1++;
        }

        var diag = $"{Get(x, y)}{Get(x + 1, y + 1)}{Get(x + 2, y + 2)}{Get(x + 3, y + 3)}";

        if (diag is "XMAS" or "SAMX")
        {
            part1++;
        }

        var diag2 = $"{Get(x, y)}{Get(x - 1, y + 1)}{Get(x - 2, y + 2)}{Get(x - 3, y + 3)}";

        if (diag2 is "XMAS" or "SAMX")
        {
            part1++;
        }
    }
}

Console.WriteLine($"Part 1: {part1}");

var part2 = 0;

for (var y = 0; y < h; y++)
{
    for (var x = 0; x < w; x++)
    {
        if (Get(x, y) != 'A')
        {
            continue;
        }

        var x1 = Get(x - 1, y - 1);

        if (x1 is not 'S' and not 'M')
        {
            continue;
        }

        var x2 = Get(x - 1, y + 1);

        if (x2 is not 'S' and not 'M')
        {
            continue;
        }

        var y1 = x1 == 'M' ? 'S' : 'M';

        if (Get(x + 1, y + 1) != y1)
        {
            continue;
        }

        var y2 = x2 == 'M' ? 'S' : 'M';

        if (Get(x + 1, y - 1) != y2)
        {
            continue;
        }

        part2++;
    }
}

Console.WriteLine($"Part 2: {part2}");