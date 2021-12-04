var effects = new Dictionary<string, int>
{
    { "magic",53 },
    {"drain",73},
    {"shield",113},
    {"poison",173},
    {"recharge",229 }
};

var min = int.MaxValue;

var player = new Player { Health = 50, Mana = 500 };
var boss = new Boss { Health = 55, Attack = 8 };
Round(player, boss, 0, false, new List<string>());
Console.WriteLine($"Part 1: {min}");
min = int.MaxValue;
Round(player, boss, 0, true, new List<string>());
Console.WriteLine($"Part 2: {min}");

void Round(Player player, Boss boss, int spent, bool part2, List<string> log)
{
    foreach (var effect in effects)
    {
        var _log = log.ToList();
        if (effect.Value > player.Mana)
        {
            continue;
        }
        if (effect.Key == "shield" && player.Shield > 1)
        {
            continue;
        }
        if (effect.Key == "recharge" && player.Recharge > 1)
        {
            continue;
        }
        if (effect.Key == "poison" && boss.Poison > 1)
        {
            continue;
        }

        var cost = spent + effect.Value;

        if (cost > min)
        {
            continue;
        }

        var p = player.Clone();
        var b = boss.Clone();

        // Player turn
        _log.Add($"-- Player turn, player: {p}, boss: {b} --");
        if (part2)
        {
            p.Health--;
            if (p.Health <= 0)
            {
                continue;
            }
        }
        ApplyEffects(p, b, _log);

        if (b.Health <= 0)
        {
            if (cost < min)
            {
                min = spent + effect.Value;
                _log.Add($"Boss died, mana spent: {min}");
            }
            continue;
        }

        if (effect.Key == "magic")
        {
            b.Health -= 4;
            _log.Add($"Player casts magic missiles, boss health is now {b.Health}");
        }
        else if (effect.Key == "drain")
        {
            b.Health -= 2;
            p.Health += 2;
            _log.Add($"Player casts drain, player health is now {p.Health} and boss health is now {b.Health}");
        }
        else if (effect.Key == "shield")
        {
            p.Shield = 6;
            _log.Add("Player casts shield");
        }
        else if (effect.Key == "poison")
        {
            b.Poison = 6;
            _log.Add("Player casts poison");
        }
        else if (effect.Key == "recharge")
        {
            p.Recharge = 5;
            _log.Add("Player casts recharge");
        }

        p.Mana -= effect.Value;

        // Boss turn
        _log.Add($"-- Boss turn, player: {p}, boss: {b} --");

        ApplyEffects(p, b, _log);

        if (b.Health <= 0)
        {
            if (cost < min)
            {
                min = spent + effect.Value;
                _log.Add($"Boss died, mana spent: {min}");
            }
            continue;
        }

        p.Health -= p.Shield > 0 ? b.Attack - 7 : b.Attack;

        if (p.Health <= 0)
        {
            continue;
        }

        _log.Add($"Player health is now {p.Health}");

        Round(p, b, spent + effect.Value, part2, _log);
    }
}

void ApplyEffects(Player p, Boss b, List<string> _log)
{
    if (p.Recharge > 0)
    {
        p.Mana += 101;
        p.Recharge--;
        _log.Add($"Recharge mana to {p.Mana}, timer is now {p.Recharge}");
    }

    if (p.Shield > 0)
    {
        p.Shield--;
        _log.Add($"Shield timer is now {p.Shield}");
    }

    if (b.Poison > 0)
    {
        b.Health -= 3;
        b.Poison--;
        _log.Add($"Poison damage to {b.Health}, timer is now {b.Poison}");
    }
}

internal class Boss
{
    public int Health { get; set; }
    public int Attack { get; set; }
    public int Poison { get; set; }

    public Boss Clone()
    {
        return new Boss
        {
            Health = Health,
            Attack = Attack,
            Poison = Poison
        };
    }

    public override string ToString()
    {
        return $"Health: {Health}";
    }
}

internal class Player
{
    public int Health { get; set; }
    public int Mana { get; set; }
    public int Shield { get; set; }
    public int Recharge { get; set; }

    public Player Clone()
    {
        return new Player
        {
            Health = Health,
            Mana = Mana,
            Shield = Shield,
            Recharge = Recharge
        };
    }

    public override string ToString()
    {
        return $"Health: {Health}, mana: {Mana}";
    }
}