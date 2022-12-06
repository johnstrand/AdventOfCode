foreach (var seq in File.ReadLines("input.txt"))
{
    var p1 = false;
    var p2 = false;
    for (var i = 4; i < seq.Length && !(p1 && p2); i++)
    {
        // Console.WriteLine($"Testing {seq.Substring(i - 4, 4)}");

        if (!p2 && i > 14 && seq.Substring(i - 14, 14).Distinct().Count() == 14)
        {
            Console.WriteLine($"Part 2: {i}");
            p2 = true;
        }

        if (!p1 && seq.Substring(i - 4, 4).Distinct().Count() == 4)
        {
            Console.WriteLine($"Part 1: {i}");
            p1 = true;
        }
    }
}