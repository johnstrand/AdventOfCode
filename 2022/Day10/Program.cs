var cycle = 0;
var reg = 1;
var pending = 0;

var code = new Queue<string>(File.ReadAllLines("input.txt"));

var checkpoint = 20;

var part1 = 0;
var x = 0;
Console.WriteLine("Part 2");
while (code.Count > 0)
{
    cycle++;

    var d = Math.Abs(reg - x);
    x++;

    Console.Write(d > 1 ? '.' : "#");
    if (x > 39)
    {
        x = 0;
        Console.WriteLine();
    }
    if (cycle == checkpoint)
    {
        part1 += cycle * reg;
        checkpoint += 40;
    }

    if (pending != 0)
    {
        reg += pending;
        pending = 0;
        continue;
    }

    var instr = code.Dequeue();

    if (instr == "noop")
    {
        continue;
    }

    pending = int.Parse(instr.Split(' ')[1]);
}

Console.WriteLine();
Console.WriteLine($"Part 1: {part1}");