using AoC.Common;

var computer = IntCode.Parse(File.ReadAllText("input.txt"));

computer.Set(1, 12, false);
computer.Set(2, 2, false);

computer.Run();

Console.WriteLine($"Part 1: {computer.Get(0, Mode.Position)}");

for (var noun = 0; noun < 100; noun++)
{
    for (var verb = 0; verb < 100; verb++)
    {
        computer.Reset();
        computer.Set(1, noun, false);
        computer.Set(2, verb, false);
        computer.Run();
        if (computer.Get(0, Mode.Position) == 19690720)
        {
            Console.WriteLine($"Part 2: {(100 * noun) + verb}");
            break;
        }
    }
}
