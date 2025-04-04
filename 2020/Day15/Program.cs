﻿foreach (var limit in new[] { 10, 2020, 30000000 })
{
    //var init = new Queue<int>(new[] { 0, 3, 6 });
    var init = new Queue<int>([12, 20, 0, 6, 1, 17, 7]);
    var seq = new List<int>();
    var seen = seq
        .Take(seq.Count - 1)
        .Select((v, i) => (v, i))
        .ToDictionary(
            x => x.v,
            x => new List<int> { x.i + 1 });

    for (var turn = 1; turn <= limit; turn++)
    {
        if (init.Count > 0)
        {
            seq.Add(init.Dequeue());
            seen[seq.Last()] = [turn];
            continue;
        }
        var last = seq[^1];
        if (seen[last].Count == 1)
        {
            seen[0].Add(turn);
            seq.Add(0);
        }
        else
        {
            var rounds = seen[last];
            var value = rounds[^1] - rounds[^2];

            if (!seen.TryGetValue(value, out var values))
            {
                values = ([]);
                seen[value] = values;
            }

            values.Add(turn);
            seq.Add(value);
        }
    }
    Console.WriteLine($"Round {limit}: {seq.Last()}");
}