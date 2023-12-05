using System.Text;

using AoC.Common;

foreach (var line in File.ReadLines("input.txt"))
{
    var sb = new StringBuilder();

    // Console.Write(line + " -> ");
    for (var i = 0; i < line.Length; i++)
    {
        if (line[i] == '(')
        {
            var length = 0;
            var repeat = 0;
            while (char.IsNumber(line[++i]))
            {
                length = (length * 10) + (line[i] - '0');
            }

            while (char.IsNumber(line[++i]))
            {
                repeat = (repeat * 10) + (line[i] - '0');
            }

            i++;

            var substring = line.Substring(i, length);
            i += length - 1;
            sb.Append(string.Concat(Enumerable.Repeat(substring, repeat)));
        }
        else
        {
            sb.Append(line[i]);
        }
    }
    var decompressed = sb.ToString();

    Console.WriteLine(decompressed.Length);
}

static long Expand(string input, int depth = 0)
{
    var length = (long)input.Length;
    var searchStart = -1;
    while ((searchStart = input.IndexOf('(', searchStart + 1)) != -1)
    {
        var end = input.IndexOf(')', searchStart);
        var marker = input.Substring(searchStart + 1, end - searchStart - 1).ToNumbers('x').ToArray();
        var markerLength = end - searchStart + 1;
        // Console.WriteLine($"{new string(' ', depth)}{input} -> {length} -= {markerLength} + {marker[0]}");
        length -= markerLength + marker[0];
        var markerContent = input.Substring(end + 1, (int)marker[0]);
        searchStart += markerLength + (int)marker[0] - 1;

        var next = Expand(markerContent, depth + 1);
        // Console.WriteLine($"{new string(' ', depth)}{length} += {marker[1]} * {next}");
        length += marker[1] * next;
    }

    // Console.WriteLine($"{new string(' ', depth)}{input} -> {length}");

    return length;
}

foreach (var line in File.ReadLines("input.txt"))
{
    // Console.WriteLine($"{line} -> {Expand(line)}");
    Console.WriteLine(Expand(line));
}