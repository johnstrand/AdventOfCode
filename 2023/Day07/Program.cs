
// See https://aka.ms/new-console-template for more information
using AoC.Common;

var values = new Dictionary<char, int>()
{
    ['A'] = 13,
    ['K'] = 12,
    ['Q'] = 11,
    ['J'] = 10,
    ['T'] = 9,
    ['9'] = 8,
    ['8'] = 7,
    ['7'] = 6,
    ['6'] = 5,
    ['5'] = 4,
    ['4'] = 3,
    ['3'] = 2,
    ['2'] = 1
};

#pragma warning disable IDE0046 // Convert to conditional expression
int GetRank2(string hand)
{
    var rank = GetRank(hand);
    foreach (var val in values!.Keys.Where(k => k != 'J'))
    {
        var newRank = GetRank(hand.Replace('J', val));
        if (newRank > rank)
        {
            rank = newRank;
        }
    }
    return rank;
}

int GetRank(string hand)
{
    var groups = hand
        .GroupBy(c => c)
        .Select(g => g.ToList())
        .ToList();

    if (groups.Count == 1)
    {
        return 7;
    }

    if (groups.Count == 2)
    {
        return groups.Any(g => g.Count == 4) ? 6 : 5;
    }

    if (groups.Count == 3)
    {
        return groups.Any(g => g.Count == 3) ? 4 : 3;
    }

    return groups.Count == 4 ? 2 : 1;
}
#pragma warning restore IDE0046 // Convert to conditional expression

int ValueOf(string hand)
{
    return hand.Select((c, i) => values![c] * (int)Math.Pow(13, 5 - i)).Sum();
}

var hands = new Dictionary<string, int>();

foreach (var line in File.ReadAllLines("input.txt"))
{
    var (hand, bet) = line.ToTuple(' ');

    Console.WriteLine($"Hand: {hand}. Rank: {GetRank(hand)}. Rank 2. {GetRank2(hand)} Value: {ValueOf(hand)}");

    hands[hand] = int.Parse(bet);
}

var rank = 1;
var part1 = 0;
foreach (var hand in hands.Keys.OrderBy(GetRank).ThenBy(ValueOf))
{
    part1 += rank * hands[hand];
    rank++;
}

Render.Result("Part 1", part1);

rank = 1;
var part2 = 0;

foreach (var k in values.Keys)
{
    if (values[k] < 11)
    {
        values[k]++;
    }
}
values['J'] = 1;

foreach (var hand in hands.Keys.OrderBy(GetRank2).ThenBy(ValueOf))
{
    part2 += rank * hands[hand];
    rank++;
}

Render.Result("Part 2", part2);
