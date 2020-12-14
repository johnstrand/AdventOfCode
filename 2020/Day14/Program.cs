using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

var mask = "";
var memory = new Dictionary<long, long>();

foreach (var row in File.ReadAllLines("input.txt"))
{
    if (row.StartsWith("mask"))
    {
        mask = row[7..];
    }
    else
    {
        var match = Regex.Match(row, @"mem\[(\d+)\] = (\d+)");
        var target = long.Parse(match.Groups[1].Value);
        var value = NumberToBinary(long.Parse(match.Groups[2].Value));
        var masked = string.Join("", Enumerable.Range(0, mask.Length).Select(n => mask[n] == 'X' ? value[n] : mask[n]));
        memory[target] = BinaryToNumber(masked);
    }
}

Console.WriteLine($"Part 1: {memory.Values.Sum()}");

memory.Clear();
mask = "";

foreach (var row in File.ReadAllLines("input.txt"))
{
    if (row.StartsWith("mask"))
    {
        mask = row[7..];
    }
    else
    {
        var match = Regex.Match(row, @"mem\[(\d+)\] = (\d+)");
        var target = NumberToBinary(long.Parse(match.Groups[1].Value));
        var value = long.Parse(match.Groups[2].Value);
        var masked = string.Join("", Enumerable.Range(0, mask.Length).Select(n => mask[n] == 'X' ? 'X' : mask[n] == '1' || target[n] == '1' ? '1' : '0'));
        var bits = masked.Count(c => c == 'X');
        foreach (var bitList in GetBits(bits))
        {
            var newAddr = BinaryToNumber(string.Join("", masked.Select(c =>
            {
                if (c != 'X')
                {
                    return c;
                }

                var v = bitList.First();
                bitList.RemoveAt(0);
                return v;
            })));
            memory[newAddr] = value;
        }
    }
}

Console.WriteLine($"Part 2: {memory.Values.Sum()}");

IEnumerable<List<char>> GetBits(int bitCount)
{
    for (var i = 0; i < (1 << bitCount); i++)
    {
        var bits = new List<char>();
        for (var b = 0; b < bitCount; b++)
        {
            bits.Add((i & (1 << b)) > 0 ? '1' : '0');
        }
        yield return bits;
    }
}

string NumberToBinary(long num)
{
    var bin = new StringBuilder();
    for (var b = 35; b >= 0; b--)
    {
        if ((num & (1L << b)) > 0)
        {
            bin.Append(1);
        }
        else
        {
            bin.Append(0);
        }
    }
    return bin.ToString();
}

long BinaryToNumber(string bin)
{
    var num = 0L;
    for (var index = 0; index < bin.Length; index++)
    {
        if (bin[index] == '1')
        {
            num += 1L << (35 - index);
        }
    }
    return num;
}
