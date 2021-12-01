// See https://aka.ms/new-console-template for more information
var values = File.ReadAllLines("input.txt").Select(int.Parse).ToList();
var current = values[0];
var increase = 0;
foreach (var value in values.Skip(1))
{
    if (value > current)
    {
        increase++;
    }

    current = value;
}

Console.WriteLine($"Part 1: {increase}.");

current = -1;
increase = 0;

for (var i = 0; i < values.Count - 2; i++)
{
    var window = values.GetRange(i, 3).Sum();
    if (current == -1)
    {
        current = window;
        continue;
    }
    if (window > current)
    {
        increase++;
    }
    current = window;
}

Console.WriteLine($"Part 2: {increase}.");
