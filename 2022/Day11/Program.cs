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
        Test = int.Parse(data.Dequeue().Split(' ')[^1]),
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

            if (part1)
            {
                nextMonkey.Items.Enqueue(item);
                continue;
            }

            var nextValue = nextMonkey.Operation.Apply(item.Level);

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

Console.WriteLine($"Part 1: {monkeys.OrderByDescending(m => m.Inspected).Take(2).Aggregate(1L, (acc, cur) => acc * cur.Inspected)}");

internal class Monkey
{
    public int Id { get; set; }
    public long Inspected { get; set; }
    public Queue<Item> Items { get; set; } = new();
    public Operation Operation { get; set; } = new("", 0);
    public int Test { get; set; }
    public int IfTrue { get; set; }
    public int IfFalse { get; set; }

    public override string ToString()
    {
        return $"{Id}. Inspected: {Inspected} ({string.Join(", ", Items)})";
    }
}

internal class Item
{
    public long Level { get; set; }

    public static IEnumerable<Item> Parse(string content)
    {
        return content.Split(':')[1].Split(",").Select(v => new Item { Level = int.Parse(v.Trim()) });
    }

    public override string ToString()
    {
        return Level.ToString();
    }
}

internal class Operation
{
    public string Operator { get; }
    public long? Operand { get; }

    public Operation(string @operator, int? operand)
    {
        Operator = @operator;
        Operand = operand;
    }

    public void Apply(Item item)
    {
        item.Level = Apply(item.Level);
    }

    public long Apply(long value)
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
        return new(parts[^2], int.TryParse(parts[^1], out var v) ? v : null);
    }
}
