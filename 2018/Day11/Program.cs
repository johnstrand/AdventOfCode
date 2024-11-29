// TODO: This is garbage
var grid = (from x in Enumerable.Range(1, 300)
            from y in Enumerable.Range(1, 300)
            select new Cell
            {
                X = x,
                Y = y,
                Power = CalculatePowerLevel(x, y, 9110)
            }).ToDictionary(c => $"{c.X}x{c.Y}", c => c);
var max = int.MinValue;
Console.Clear();
Console.WriteLine();
Console.WriteLine("[");
Console.WriteLine("[");
Console.WriteLine("[");
for (var y = 1; y <= 300; y++)
{
    Console.SetCursorPosition((y * 100 / 300) + 1, 1);
    Console.WriteLine("*]");
    for (var x = 1; x <= 300; x++)
    {
        Console.SetCursorPosition((x * 100 / 300) + 1, 2);
        Console.WriteLine("*]");
        for (var size = 1; size <= 301 - Math.Max(x, y); size++)
        {
            Console.SetCursorPosition(1, 3);
            Console.WriteLine($"{size}]");
            var sum = Sum(grid, x, y, size);
            if (sum > max)
            {
                Console.SetCursorPosition(1, 4);
                Console.WriteLine($"[{x},{y}] = {sum} ({size})");
                max = sum;
            }
        }
    }
}
Console.Read();

int CalculatePowerLevel(int x, int y, int serialNo)
{
    var rackId = x + 10;
    var powerLevel = rackId * y;
    powerLevel += serialNo;
    powerLevel *= rackId;

    return ((powerLevel / 100) % 10) - 5;
}

int Sum(Dictionary<string, Cell> grid, int x, int y, int size)
{
    return (from tx in Enumerable.Range(x, size)
            from ty in Enumerable.Range(y, size)
            let key = $"{tx}x{ty}"
            select grid.TryGetValue(key, out var value) ? value.Power : 0).Sum();
}

internal class Cell
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Power { get; set; }
}
