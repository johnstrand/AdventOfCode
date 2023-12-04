using System.Text;

foreach (var line in File.ReadLines("input-test.txt"))
{
    var sb = new StringBuilder();

    Console.Write(line + " -> ");
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
            // i += length - 1;
            sb.Append(string.Concat(Enumerable.Repeat(substring, repeat)));
        }
        else
        {
            sb.Append(line[i]);
        }
    }
    var decompressed = sb.ToString();

    Console.WriteLine(decompressed.Length);
    //Console.WriteLine($"{decompressed} ({decompressed.Length})");
}