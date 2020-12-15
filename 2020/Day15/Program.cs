using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

foreach (var limit in new[] { 10, 2020, 30000000 })
{
    //var init = new Queue<int>(new[] { 0, 3, 6 });
    var init = new Queue<int>(new[] { 12, 20, 0, 6, 1, 17, 7 });
    var seq = new List<int>();
    var seen = seq
        .Take(seq.Count - 1)
        .Select((v, i) => (v, i))
        .ToDictionary(
            x => x.v,
            x => new List<int> { x.i + 1 });

    for (var turn = 1; turn <= limit; turn++)
    {
        if (init.Any())
        {
            seq.Add(init.Dequeue());
            seen[seq.Last()] = new List<int> { turn };
            continue;
        }
        var last = seq[seq.Count - 1];
        if (seen[last].Count == 1)
        {
            seen[0].Add(turn);
            seq.Add(0);
        }
        else
        {
            var rounds = seen[last];
            var value = rounds[rounds.Count - 1] - rounds[rounds.Count - 2];

            if (!seen.ContainsKey(value))
            {
                seen[value] = new List<int>();
            }

            seen[value].Add(turn);
            seq.Add(value);
        }
    }
    Console.WriteLine($"Round {limit}: {seq.Last()}");
}