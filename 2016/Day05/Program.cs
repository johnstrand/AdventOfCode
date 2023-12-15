using System.Security.Cryptography;
using System.Text;

var seed = "abbhdwsy";
//var seed = "abc";
var index = 0;

using var md5 = MD5.Create();
var password1 = new List<char>(8);
var password2 = Enumerable.Repeat('\0', 8).ToList();

Console.Clear();
while (true)
{
#pragma warning disable CA1850 // Prefer static 'HashData' method over 'ComputeHash'
    var hash = md5.ComputeHash(Encoding.ASCII.GetBytes($"{seed}{index}"));
#pragma warning restore CA1850 // Prefer static 'HashData' method over 'ComputeHash'
    if (CheckHash(hash, out var first, out var second))
    {
        var letter = first.ToString("x")[0];
        if (password1.Count < 8)
        {
            Console.SetCursorPosition(password1.Count, 0);
            Console.Write(letter);
            password1.Add(letter);
        }

        if (first < 8 && password2[first] == '\0')
        {
            password2[first] = second.ToString("x")[0];
            Console.SetCursorPosition(first, 1);
            Console.Write(second.ToString("x")[0]);
        }

        if (password1.Count == 8 && password2.All(c => c != '\0'))
        {
            break;
        }
    }
    index++;
}

Console.WriteLine();

static bool CheckHash(byte[] hash, out byte first, out byte second)
{
    first = second = 0;
    var match = hash[0] == 0 && hash[1] == 0 && hash[2] < 0x10;
    if (match)
    {
        first = (byte)(hash[2] & 0xF);
        second = (byte)(hash[3] >> 4);
    }
    return match;
}
