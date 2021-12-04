using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

var bits = ToBitArraySum(File.ReadAllLines("input-test.txt"), v => v > 0);
var v1 = ToInt(bits);
var v2 = ToInt(bits.Not());
Console.WriteLine($"Part 1: {v1 * v2}");

var candidates = File.ReadAllLines("input-test.txt");


void ApplyFilter(int bit, IEnumerable<string> candidates, Func<int, bool> condition)
{

}

BitArray ToBitArray(string input)
{
    var arr = new BitArray(input.Length);
    for (int digit = 0; digit < input.Length; digit++)
    {
        arr[digit] = input[digit] == '1';
    }
    return arr;
}

BitArray ToBitArraySum(IEnumerable<string> input, Func<int, bool> condition)
{
    var digits = new List<int>();
    foreach (var row in input)
    {
        for (int digit = 0; digit < row.Length; digit++)
        {
            if (digits.Count == digit)
            {
                digits.Add(0);
            }
            digits[digit] += row[digit] == '1' ? 1 : -1;
        }
    }

    return new BitArray(digits.Select(condition).ToArray());
}

int ToInt(BitArray bits)
{
    var sum = 0;
    for (var index = 0; index < bits.Count; index++)
    {
        var value = bits[bits.Count - index - 1] ? 1 : 0;
        sum += value << index;
    }
    return sum;
}
