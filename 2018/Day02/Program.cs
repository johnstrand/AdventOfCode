var two = 0;
var three = 0;
var candidates = new List<string>();
foreach (var row in File.ReadAllLines("input.txt"))
{
    var g = row.GroupBy(c => c);
    if (g.Any(x => x.Count() == 2))
    {
        two++;
    }
    if (g.Any(x => x.Count() == 3))
    {
        three++;
    }
    foreach (var candidate in candidates)
    {
        var filtered = row.Zip(candidate, (c1, c2) => c1 == c2 ? c1 : ' ').Where(c => c != ' ');
        if (filtered.Count() == row.Length - 1)
        {
            Console.WriteLine($"{row} :: {candidate}");
            Console.WriteLine(string.Join("", filtered));
        }
    }
    candidates.Add(row);
}

Console.WriteLine($"{two} * {three} = {two * three}");