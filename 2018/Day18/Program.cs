// TODO: This is a mess

Dictionary<int, List<int>> surroundingIndices = new();

for (var index = 0; index < 50 * 50; index++)
{
    surroundingIndices.Add(index, SurroundingIndices(index, 50, 50).ToList());
}
var state = string.Concat(File.ReadAllLines("Input.txt")).ToCharArray();
var limit = 100000000;
var next = 0;
var trees = 0;
var yards = 0;
for (var min = 0; min < limit; min++)
{
    Display(state, 50);
    //Console.WriteLine();
    //Console.WriteLine($"Time slice: {min}");
    //Thread.Sleep(1000);
    if (min == next)
    {
        next += 1000;
        trees = state.Count(c => c == '|');
        yards = state.Count(c => c == '#');
        Console.WriteLine($"Resource count at index {min}: {trees} * {yards} = {trees * yards}. ");
    }

    state = Mutate(state, 50, 50);
}
trees = state.Count(c => c == '|');
yards = state.Count(c => c == '#');
Console.Write($"Resource count: {trees} * {yards} = {trees * yards}. ");

Console.Read();

// TODO: Figure it out
void Display(char[] state, int w)
{
    //Console.SetCursorPosition(0, 0);
    //var b = string.Join("", state.Select((c, i) => c.ToString() + (i > 0 && i % w == 0 ? Environment.NewLine : "")));
    //Console.Write(b);
}

char[] Mutate(char[] state, int w, int h)
{
    return state.Select((grid, index) =>
        Map(grid, surroundingIndices[index].Select(i => state[i]).ToArray())).ToArray();
}

char Map(char value, char[] surrounding)
{
    /*
    var s = new Dictionary<char, int>();
    foreach(var c in surrounding)
    {
        if(s.ContainsKey(c))
        {
            s[c]++;
        }
        else
        {
            s.Add(c, 1);
        }
    }*/
    if (value == '.')
    {
        return HasTwo(surrounding, '|') ? '|' : '.';
    }

    if (value == '|')
    {
        return HasTwo(surrounding, '#') ? '#' : '|';
    }

    if (value == '#')
    {
        return BothOf(surrounding, '#', '|') ? '#' : '.';
    }
    return value;
}

bool BothOf(char[] list, char c1, char c2)
{
    var i1 = false;
    var i2 = false;
    foreach (var item in list)
    {
        i1 = i1 || item == c1;
        i2 = i2 || item == c2;
        if (i1 && i2)
        {
            return true;
        }
    }

    return false;
}

bool HasTwo(char[] list, char target)
{
    var count = 0;
    foreach (var item in list)
    {
        if (item == target)
        {
            count++;
            if (count > 2)
            {
                return true;
            }
        }
    }

    return false;
}

IEnumerable<int> SurroundingIndices(int index, int w, int h)
{
    var x = index % w;
    var y = index / w;

    for (var ty = Math.Max(0, y - 1); ty <= Math.Min(y + 1, h - 1); ty++)
    {
        for (var tx = Math.Max(0, x - 1); tx <= Math.Min(x + 1, w - 1); tx++)
        {
            if (tx == x && ty == y)
            {
                continue;
            }

            yield return (ty * w) + tx;
        }
    }
}
