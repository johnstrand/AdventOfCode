using AoC.Common;

var grid = Grid<int>.FromRows(File.ReadAllLines("input.txt"));

var flashes = 0;
var stepAllFlashed = 0;

var step = 0;
while (true)
{
    Console.Clear();
    Console.WriteLine($"After step 100: {flashes} flashes. All flashed: Step {stepAllFlashed}");
    /*
    Console.WriteLine();
    grid.Display();
    Console.WriteLine();
    */
    if (stepAllFlashed > 0)
    {
        break;
    }
    // await Task.Delay(100);

    grid.ForEach((_, v) => v + 1);

    var toFlash = new Queue<(int x, int y)>(grid.GetMatching(v => v > 9));
    while (toFlash.Count > 0)
    {
        var (x, y) = toFlash.Dequeue();
        foreach (var (ax, ay) in grid.GetAdjacent(x, y))
        {
            if (grid.GetValue(ax, ay) > 9)
            {
                continue;
            }

            var value = grid.SetValue(ax, ay, v => v + 1);
            if (value > 9)
            {
                toFlash.Enqueue((ax, ay));
            }
        }
    }

    var stepFlashes = grid.GetMatching(v => v > 9).Count();

    if (stepFlashes == grid.Count)
    {
        stepAllFlashed = step + 1;
    }

    if (step < 100)
    {
        flashes += stepFlashes;
    }

    grid.ForEach((_, v) => v = v > 9 ? 0 : v);
    step++;
}
