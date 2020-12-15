using System;
using System.Collections.Generic;
using System.Linq;

foreach (var limit in new[] { 10, 2020, /*30000000*/ })
{
    //var seq = new List<int> { 12, 20, 0, 6, 1, 17, 7 };
    var seq = new List<int> { 0, 3, 6 };
    var seen = seq
        .Take(seq.Count - 1)
        .Select((v, i) => (v, i))
        .ToDictionary(
            x => x.v,
            x => new List<int> { x.i + 1 });

    if (!seen.ContainsKey(0))
    {
        seen[0] = new List<int>();
    }

    for (var turn = seq.Count + 1; turn <= limit; turn++)
    {
        var last = seq.Last();
        Console.WriteLine($"Last value: {last}");
        if (seen.ContainsKey(last))
        {
            var rounds = seen[last].Take(2).ToList();
            var value = rounds[0] - rounds[1];
            if (!seen.ContainsKey(value))
            {
                seen[value] = new List<int>();
            }
            seen[value].Insert(0, turn);
            seq.Add(value);
        }
        else
        {
            seen[0] = new List<int> { turn };
            seen[0].Insert(0, turn);
            seq.Add(0);
        }
        /*
        if (!lastTurns.ContainsKey(last))
        {
            lastTurns[last] = new List<int>();
        }
        lastTurns[last].Insert(0, turn);
        if (seen.Add(last))
        {
            seq.Add(0);
        }
        else
        {
            var rounds = lastTurns[last].Take(2).ToList();
            seq.Add(rounds.First() - rounds.Last());
        }
        */
    }
    Console.WriteLine($"Round {limit}: {seq.Last()}");
}