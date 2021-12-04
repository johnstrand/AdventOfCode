using System.Text.Json;

var input = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText("input.txt"));

var part1 = Sum(input, false);
var part2 = Sum(input, true);

Console.WriteLine($"Part 1: {part1}, part 2: {part2}");

int Sum(JsonElement element, bool skipRed)
{
    switch (element.ValueKind)
    {
        case JsonValueKind.Object:
            {
                // TODO: Make all of this more readable
                return skipRed && element.EnumerateObject().Any(p => p.Value.ValueKind == JsonValueKind.String && p.Value.GetString() == "red")
                    ? 0
                    : element.EnumerateObject().Sum(p => Sum(p.Value, skipRed));
            }

        case JsonValueKind.Array:
            {
                return element.EnumerateArray().Sum(e => Sum(e, skipRed));
            }

        case JsonValueKind.Number:
            return element.GetInt32();
        default:
            return 0;
    }
}