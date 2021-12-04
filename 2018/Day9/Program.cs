// TODO: This is not complete
var marbles = new List<int> { 0 };
var active = 0;
var next = 1;
var players = 10;
var player = 0;
var limit = 1618;
var score = Enumerable.Range(0, players).ToDictionary(x => x, _ => 0);
while (next < limit + 1)
{
    if (next % 23 == 0)
    {
        var removed = active - 7;
        if (removed < 0)
        {
            removed += marbles.Count;
        }
        score[player] += 23 + marbles[removed];
        marbles.RemoveAt(removed);
        active = removed;
    }
    else
    {
        active += 2;
        if (active > marbles.Count)
        {
            active -= marbles.Count;
        }
        marbles.Insert(active, next);
    }

    //Console.WriteLine($"[{player + 1}] {string.Join(" ", marbles.Select((x, i) => i != active ? x.ToString() : $"({x})"))}");
    player = (player + 1) % players;
    //Thread.Sleep(100);
    next++;
}
foreach (var item in score.OrderBy(kv => kv.Key))
{
    Console.WriteLine($"Player {item.Key + 1}: {item.Value}");
}
Console.Read();