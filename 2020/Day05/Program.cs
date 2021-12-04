using System;
using System.IO;
using System.Linq;

var seats = File.ReadAllLines("input.txt").Select(GetSeat).OrderBy(s => Index(s)).ToList();

var part1 = seats.Skip(seats.Count - 1).Max(Index);

var part2 = Index(seats.Where((seat, index) => Index(seat) + 1 != Index(seats[index + 1])).First());

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2 + 1}");

int Index((int row, int col) seat)
{
    return (seat.row * 8) + seat.col;
}

(int row, int col) GetSeat(string seq)
{
    return (
        BinarySearch(seq[0..7]),
        BinarySearch(seq[7..]));
}

int BinarySearch(string seq)
{
    var min = 0;
    var max = (int)Math.Pow(2, seq.Length) - 1;
    foreach (var c in seq)
    {
        if (c == 'F' || c == 'L')
        {
            max = (max + min) / 2;
        }
        else
        {
            min = (max + min) / 2;
        }
    }
    return max;
}