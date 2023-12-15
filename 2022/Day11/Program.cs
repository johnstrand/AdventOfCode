#define PART1

var monkeys = new List<Monkey>();
var data = new Queue<string>(File.ReadAllLines("input-test.txt"));

var part1 = false;

while (data.Count > 0)
{
    var id = int.Parse(data.Dequeue().Split(' ')[1].Trim(':'));
    var m = new Monkey
    {
        Id = id,
        Items = new(Item.Parse(data.Dequeue())),
        Operation = Operation.Parse(data.Dequeue()),
        Test = uint.Parse(data.Dequeue().Split(' ')[^1]),
        IfTrue = int.Parse(data.Dequeue().Split(' ')[^1]),
        IfFalse = int.Parse(data.Dequeue().Split(' ')[^1]),
    };
    monkeys.Add(m);
    while (data.Count > 0 && string.IsNullOrEmpty(data.Peek()))
    {
        data.Dequeue();
    }
}

for (var round = 0; round < (part1 ? 20 : 1_000); round++)
{
    foreach (var monkey in monkeys)
    {
        while (monkey.Items.Count > 0)
        {
            var item = monkey.Items.Dequeue();
            // Console.WriteLine(item.Level);
            monkey.Inspected++;
            //Console.Write($"{monkey.Id}: {item} -> ");
            monkey.Operation.Apply(item);
            //Console.Write($"{item} -> ");
            if (part1)
            {
                item.Level /= 3;
            }
            //Console.WriteLine(item);

            var nextMonkey = monkeys[item.Level % monkey.Test == 0 ? monkey.IfTrue : monkey.IfFalse];

            if (part1 || nextMonkey.Operation.Operator == "+")
            {
                nextMonkey.Items.Enqueue(item);
                continue;
            }

            var nextValue = nextMonkey.Operation.Apply(item.Level);

            var tempValue = 1U;

            foreach (var m in monkeys)
            {
                if (m.Test % nextValue == 0)
                {
                    tempValue *= m.Test;
                }
            }

            item.Level = tempValue;

            /*
            if (nextValue % nextMonkey.Test == 0)
            {
                item.Level = nextMonkey.Test;
            }
            */

            nextMonkey.Items.Enqueue(item);

            /*
            if ((item.Level % monkey.Test) == 0)
            {
                monkeys[monkey.IfTrue].Items.Enqueue(item);
            }
            else
            {
                monkeys[monkey.IfFalse].Items.Enqueue(item);
            }
            */
        }
    }

    Console.Write($"Round {round} ");
    foreach (var monkey in monkeys)
    {
        Console.Write($" {monkey.Inspected}");
    }
    Console.WriteLine();
}

Console.WriteLine();
#if PART2
Console.WriteLine($"Part 1: {monkeys.OrderByDescending(m => m.Inspected).Take(2).Aggregate(new decimal(1), (acc, cur) => acc * cur.Inspected)}");
#else
Console.WriteLine($"Part 2: {monkeys.OrderByDescending(m => m.Inspected).Take(2).Aggregate(1L, (acc, cur) => acc * cur.Inspected)}");
#endif

internal class Monkey
{
    public int Id { get; set; }
    public Queue<Item> Items { get; set; } = new();
    public Operation Operation { get; set; } = new("", 0);
    public int IfTrue { get; set; }
    public int IfFalse { get; set; }
    public int Inspected { get; set; }

#if PART2
    public decimal Test { get; set; }
#else
    public uint Test { get; set; }
#endif

    public override string ToString()
    {
        return $"{Id}. Inspected: {Inspected} ({string.Join(", ", Items)})";
    }
}

internal class Item
{
#if PART2
    public decimal Level { get; set; }
#else
    public ulong Level { get; set; }
#endif

    public static IEnumerable<Item> Parse(string content)
    {
        return content.Split(':')[1].Split(",").Select(v => new Item { Level = uint.Parse(v.Trim()) });
    }

    public override string ToString()
    {
        return Level.ToString();
    }
}

internal class Operation(string @operator, uint? operand)
{
    public string Operator { get; } = @operator;
#if PART2
    public decimal? Operand { get; }
#else
    public ulong? Operand { get; } = operand;

#endif

    public void Apply(Item item)
    {
        item.Level = Apply(item.Level);
    }

#if PART2
    public decimal Apply(decimal value)
#else
    public ulong Apply(ulong value)
#endif
    {
        return Operator switch
        {
            "+" => value + (Operand ?? value),
            _ => value * (Operand ?? value)
        };
    }

    public static Operation Parse(string content)
    {
        var parts = content.Split(' ');
        return new(parts[^2], uint.TryParse(parts[^1], out var v) ? v : null);
    }
}
