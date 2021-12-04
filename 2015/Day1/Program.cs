var input = File.ReadAllText("Input.txt");
var index = 0;
var first = true;
var pos = input.Aggregate(0, (acc, cur) =>
{
    if (acc == -1 && first)
    {
        Console.WriteLine(index);
        first = false;
    }
    index++;
    return acc + (cur == '(' ? 1 : -1);
});

Console.WriteLine(pos);

Console.ReadLine();
