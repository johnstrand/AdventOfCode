bool part2 = true;
bool test = false;

int sum = 0;

Dictionary<string, char> namedDigits = new()
{
    //["zero"] = '0',
    ["one"] = '1',
    ["two"] = '2',
    ["three"] = '3',
    ["four"] = '4',
    ["five"] = '5',
    ["six"] = '6',
    ["seven"] = '7',
    ["eight"] = '8',
    ["nine"] = '9'
};

string input = (test, part2) switch
{
    (true, false) => "input-test.txt",
    (true, true) => "input-test2.txt",
    _ => "input.txt"
};

foreach (string line in File.ReadLines(input))
{
    char[] values = new char[2];
    Console.WriteLine(line);
    Console.Write("\t");
    for (int i = 0; i < line.Length; i++)
    {
        char chr = line[i];
        if (char.IsDigit(chr))
        {
            if (values[0] == '\0')
            {
                values[0] = chr;
            }
            values[1] = chr;
            Console.Write($"{chr} ");
            continue;
        }

        if (part2)
        {
            foreach (KeyValuePair<string, char> namedDigit in namedDigits)
            {
                if (line.IndexOf(namedDigit.Key, i) == i)
                {
                    if (values[0] == '\0')
                    {
                        values[0] = namedDigit.Value;
                    }
                    Console.Write($"{namedDigit.Value} ");
                    values[1] = namedDigit.Value;
                    //i += namedDigit.Key.Length - 1;
                    break;
                }
            }
        }
    }
    Console.WriteLine();

    sum += int.Parse(new string(values));
}

Console.WriteLine($"Result: {sum}");
