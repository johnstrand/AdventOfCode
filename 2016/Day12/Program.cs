using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

foreach (var part1 in new[] { true, false })
{
    var registers = new Dictionary<char, int>
{
    { 'a', 0 },
    { 'b', 0 },
    { 'c', part1 ? 0 : 1 },
    { 'd', 0 },
};

    var instr = File.ReadAllLines("input.txt").Select(r => r.Split(' ')).ToList();
    var ptr = 0;

    int ValueOf(string token)
    {
        if (char.IsLetter(token[0]))
        {
            return registers[token[0]];
        }

        return int.Parse(token);
    }

    while (ptr < instr.Count)
    {
        var cur = instr[ptr];
        switch (cur[0])
        {
            case "cpy":
                registers[cur[2][0]] = ValueOf(cur[1]);
                break;
            case "inc":
                registers[cur[1][0]]++;
                break;
            case "dec":
                registers[cur[1][0]]--;
                break;
            case "jnz":
                if (ValueOf(cur[1]) != 0)
                {
                    ptr += ValueOf(cur[2]);
                    continue;
                }
                break;
        }
        ptr++;
    }

    Console.WriteLine($"Part {(part1 ? 1 : 2)}");
    foreach (var reg in registers)
    {
        Console.WriteLine($"{reg.Key}: {reg.Value}");
    }
}
