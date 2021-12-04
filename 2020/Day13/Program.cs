using System;
using System.Linq;

//var input = new[] { "939", "7,13,x,x,59,x,31,19" };

// TODO: This doesn't seem right

var input = new[] { "1007268", "17,x,x,x,x,x,x,41,x,x,x,x,x,x,x,x,x,937,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,13,x,x,x,x,23,x,x,x,x,x,29,x,397,x,x,x,x,x,37,x,x,x,x,x,x,x,x,x,x,x,x,19" };

var currentTime = int.Parse(input[0]);

var times = input.Last().Split(',').Where(IsNumber).Select(int.Parse).ToList();

var ts = currentTime;
while (true)
{
    var num = times.Find(t => ts % t == 0);
    if (num > 0)
    {
        Console.WriteLine($"Part 1: {num * (ts - currentTime)}");
        break;
    }
    ts++;
}

var t = 0L;
var rules = input.Last().Split(',').Select((n, i) => n == "x" ? (-1, -1) : (value: long.Parse(n), offset: i)).Where(x => x.value != -1).ToList();
var missing = 1;
var multiplier = rules[0].value;

while (missing < rules.Count)
{
    t += multiplier;
    var next = rules[missing];
    if ((t + next.offset) % next.value == 0)
    {
        multiplier *= next.value;
        missing++;
    }
}

Console.WriteLine($"Part 2: {t}");

var (value, offset) = rules.First(r => r.value == rules.Max(r => r.value));

var s = 0L;
while (true)
{
    s += value;
    if (s + offset == 626670513163231)
    {
    }
}

static bool IsNumber(string str)
{
    return str.All(char.IsDigit);
}