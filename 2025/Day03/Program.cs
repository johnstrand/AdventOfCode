using AoC.Common;

var part1 = 0;
foreach (var bank in Input.ReadTest())
{
    var max = 0;
    for (var i = 0; i < bank.Length; i++)
    {
        for (var j = i + 1; j < bank.Length; j++)
        {
            var value = (bank[i] - '0') * 10 + (bank[j] - '0');

            if (value > max)
            {
                max = value;
            }
        }
    }
    Console.WriteLine($"Bank: {bank[0..10]} => Max pair value: {max}");

    part1 += max;
}

Console.WriteLine($"Part 1: {part1}");
