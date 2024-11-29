// TODO: Clean this up
var min = 387638;
var max = 919123;
var valid = 0;
for (var current = min; current <= max; current++)
{
    if (Valid(current))
    {
        valid++;
    }
}

Console.WriteLine(valid);

bool Valid(int no)
{
    var segs = no.ToString().Select(c => c - '0').ToList();
    var pair = false;
    for (var i = 0; i < segs.Count - 1; i++)
    {
        if (segs[i] > segs[i + 1])
        {
            return false;
        }
        pair = pair || CheckPair(segs, i);
    }

    return pair;
}

bool CheckPair(List<int> list, int index)
{
    return list[index] == list[index + 1] &&
        // Part 2
        (index == 0 || list[index - 1] != list[index]) &&
        (index >= list.Count - 2 || list[index + 2] != list[index]);
}