// See https://aka.ms/new-console-template for more information

var pos = (x: 0, y: 0);
foreach (var line in File.ReadAllLines("input.txt"))
{
    var parts = line.Split(' ');
    var dir = parts[0];
    var dist = int.Parse(parts[1]);

    pos = dir switch
    {
        "forward" => (pos.x + dist, pos.y),
        "down" => (pos.x, pos.y + dist),
        "up" => (pos.x, pos.y - dist),
        _ => pos
    };

    //Console.WriteLine(pos);
}

Console.WriteLine($"Part 1: {pos.x * pos.y}");

var state = (x: 0, y: 0, aim: 0);

foreach (var line in File.ReadAllLines("input.txt"))
{
    var parts = line.Split(' ');
    var dir = parts[0];
    var dist = int.Parse(parts[1]);

    state = dir switch
    {
        "forward" => (state.x + dist, state.y + (state.aim * dist), state.aim),
        "down" => (state.x, state.y, state.aim + dist),
        "up" => (state.x, state.y, state.aim - dist),
        _ => state
    };

    //Console.WriteLine(pos);
}

Console.WriteLine($"Part 2: {state.x * state.y}");
