using AoC.Common;

var computer = IntCode.Parse(File.ReadAllText("input.txt"));

computer.SetInput(() => 1);
computer.SetOutput(Console.WriteLine);
computer.Run(false);
Console.WriteLine();
