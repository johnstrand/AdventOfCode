using AoC.Common;

var dial = 50;

IEnumerable<int> TurnLeft(int offset)
{
    for (int i = 0; i < offset; i++)
    {
        if (dial == 0)
        {
            yield return 0;
        }
        dial--;
        if (dial < 0)
        {
            dial = 99;
        }
    }
}

IEnumerable<int> TurnRight(int offset)
{
    for (int i = 0; i < offset; i++)
    {
        if (dial == 0)
        {
            yield return 0;
        }
        dial++;
        if (dial > 99)
        {
            dial = 0;
        }
    }
}

var password = 0;
var password2 = 0;
foreach (var command in Input.ReadActual())
{
    var direction = command[0];
    var offset = int.Parse(command[1..]);

    if (direction == 'L')
    {
        password2 += TurnLeft(offset).Count();
    }
    else if (direction == 'R')
    {
        password2 += TurnRight(offset).Count();
    }

    if (dial == 0)
    {
        password++;
    }
}

Console.WriteLine($"Part 1: The password is {password}");
Console.WriteLine($"Part 2: The password is {password2}");