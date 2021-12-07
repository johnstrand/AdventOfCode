using System.Text.RegularExpressions;

var display = Enumerable.Repeat(' ', 7 * 3).ToList();

void Show()
{
    for (var offset = 0; offset < display.Count; offset += 7)
    {
        Console.WriteLine(new string(display.GetRange(offset, 7).ToArray()));
    }
}

foreach (var cmd in File.ReadAllLines("input-test.txt"))
{
    Match m;
    if ((m = Regex.Match(cmd, @"^rect (\d+)x(\d+)$")).Success)
    {
        for (var y = 0; y < int.Parse(m.Groups[2].Value); y++)
        {
            for (var x = 0; x < int.Parse(m.Groups[1].Value); x++)
            {
                display[y * 7 + x] = '#';
            }
        }
    }
    else if ((m = Regex.Match(cmd, @"^rotate (.+?)=(\d+) by (\d+)$")).Success)
    {
        var type = m.Groups[1].Value[^1];
        var val = int.Parse(m.Groups[2].Value);
        var dist = int.Parse(m.Groups[3].Value);

    }
    Show();
}
