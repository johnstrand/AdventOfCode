foreach (var part1 in new[] { true, false })
{
    var field = File.ReadAllLines("input.txt").Select(r => r.ToArray()).ToArray();
    var w = field[0].Length;
    var h = field.Length;

    var anyChanged = true;
    while (anyChanged)
    {
        anyChanged = false;
        var next = Copy(field);
        for (var y = 0; y < h; y++)
        {
            for (var x = 0; x < w; x++)
            {
                var state = GetState(x, y, field);
                var neighbors = part1 ? Neighbors(x, y, field) : Neighbors2(x, y, field);
                //var changed = new bool?();
                if (state == 'L' && neighbors == 0)
                {
                    next[y][x] = '#';
                    //changed = true;
                    anyChanged = true;
                }
                else if (state == '#' && neighbors >= (part1 ? 4 : 5))
                {
                    next[y][x] = 'L';
                    //changed = false;
                    anyChanged = true;
                }
                /*
                if (changed.HasValue)
                {
                    Console.ForegroundColor = changed.Value ? ConsoleColor.Green : ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.Write(next[y][x]);
                */
            }
            //Console.WriteLine();
        }
        field = next;
        //Console.WriteLine();
    }

    Console.WriteLine($"Part {(part1 ? 1 : 2)}: {CountOccupied(field)}");
}

char[][] Copy(char[][] field)
{
    return field.Select(f => f.ToArray()).ToArray();
}

int Neighbors2(int x, int y, char[][] field)
{
    var sum = 0;
    foreach (var dy in Enumerable.Range(-1, 3))
    {
        foreach (var dx in Enumerable.Range(-1, 3))
        {
            if (dy == 0 && dx == 0)
            {
                continue;
            }
            var tx = x + dx;
            var ty = y + dy;
            while (GetState(tx, ty, field) == '.')
            {
                tx += dx;
                ty += dy;
            }
            if (GetState(tx, ty, field) == '#')
            {
                sum++;
            }
        }
    }
    return sum;
}
int Neighbors(int x, int y, char[][] field)
{
    var sum = 0;
    for (var dy = y - 1; dy <= y + 1; dy++)
    {
        if (dy < 0 || dy == field.Length)
        {
            continue;
        }
        for (var dx = x - 1; dx <= x + 1; dx++)
        {
            if (dx < 0 || (dx == x && dy == y) || dx == field[dy].Length)
            {
                continue;
            }
            if (field[dy][dx] == '#')
            {
                sum++;
            }
        }
    }
    return sum;
}

int CountOccupied(char[][] field)
{
    return field.Sum(r => r.Sum(item => item == '#' ? 1 : 0));
}

char GetState(int x, int y, char[][] field)
{
    if (y < 0 || y >= field.Length)
    {
        return '\0';
    }

    if (x < 0 || x >= field[y].Length)
    {
        return '\0';
    }

    return field[y][x];
}
