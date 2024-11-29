using AoC.Common;

var computer = IntCode.Parse(File.ReadAllText("input.txt"));

computer.SetInput(() => 1);
computer.Run(false);
Console.WriteLine();

computer.Reset();

computer.SetInput(() => 5);
computer.Run(false);
