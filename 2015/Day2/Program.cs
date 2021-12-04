var (paper, ribbon) = File.ReadAllLines("Input.txt")
    .Select(row => row.Split('x').Select(int.Parse).OrderBy(n => n).ToArray())
    .Select(sides =>
    {
        var l = sides[0];
        var h = sides[1];
        var w = sides[2];
        return (paper: (2 * l * w) + (2 * w * h) + (3 * h * l),
            ribbon: (2 * h) + (2 * l) + (l * h * w));
    })
    .Aggregate((acc, cur) => (paper: acc.paper + cur.paper, ribbon: acc.ribbon + cur.ribbon));

Console.WriteLine($"Paper: {paper}. Ribbon: {ribbon}");
Console.ReadLine();
