using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var data = new Queue<string>(File.ReadLines("input.txt"));
var passport = new Dictionary<string, string>();
var part1 = 0;
while (data.Count > 0)
{
    var next = data.Dequeue();
    if (next == string.Empty)
    {
        if (passport.Count == 8 || (passport.Count == 7 && !passport.ContainsKey("cid")))
        {
            part1++;
        }
        passport.Clear();
    }
    else
    {
        next.Split(' ').Select(item => item.Split(':')).ToList().ForEach(pair => passport.Add(pair[0], pair[1]));
    }
}
if (passport.Count == 8 || (passport.Count == 7 && !passport.ContainsKey("cid")))
{
    part1++;
}

Console.WriteLine($"Part 1: {part1}");