using System;
using System.IO;
using System.Linq;
using System.Text.Json;

var input = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText("input.txt"));

var part1 = Sum(input, false);
var part2 = Sum(input, true);

Console.WriteLine($"Part 1: {part1}, part 2: {part2}");

int Sum(JsonElement element, bool skipRed)
{
    if (element.ValueKind == JsonValueKind.Object)
    {
        if (skipRed && element.EnumerateObject().Any(p => p.Value.ValueKind == JsonValueKind.String && p.Value.GetString() == "red"))
        {
            return 0;
        }
        return element.EnumerateObject().Sum(p => Sum(p.Value, skipRed));
    }
    else if (element.ValueKind == JsonValueKind.Array)
    {
        return element.EnumerateArray().Sum(e => Sum(e, skipRed));
    }
    else if (element.ValueKind == JsonValueKind.Number)
    {
        return element.GetInt32();
    }
    else
    {
        return 0;
    }
}