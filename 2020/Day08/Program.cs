using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var instr = File.ReadAllLines("input.txt").Select(r => r.Split(' ')).Select(r => (op: r[0], offset: int.Parse(r[1]))).ToList();

Console.WriteLine($"Part 1: {Run(instr, out var _)}");

var indices = instr.Select((x, i) => x.op == "nop" || x.op == "jmp" ? i : -1).Where(o => o > -1).ToList();

foreach (var index in indices)
{
    var acc = Run(instr.Select((ins, ix) =>
    {
        if (ix != index)
        {
            return ins;
        }

        return ins.op == "nop" ? ("jmp", ins.offset) : ("nop", ins.offset);
    }).ToList(), out var exited);

    if (exited)
    {
        Console.WriteLine($"Part 2: {acc}");
        break;
    }
}

static int Run(List<(string op, int offset)> instr, out bool exited)
{
    var seen = new HashSet<int>();
    var ptr = 0;
    var acc = 0;
    exited = false;
    while (ptr > -1 && ptr < instr.Count)
    {
        if (!seen.Add(ptr))
        {
            return acc;
        }
        var (op, offset) = instr[ptr];
        switch (op)
        {
            case "nop":
                ptr++;
                break;

            case "jmp":
                ptr += offset;
                break;

            case "acc":
                acc += offset;
                ptr++;
                break;
        }
    }

    exited = true;
    return acc;
}