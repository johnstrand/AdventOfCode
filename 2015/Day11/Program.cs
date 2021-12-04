var password = new List<char> { 'h', 'x', 'b', 'x', 'w', 'x', 'b', 'a' };
var matches = 0;
while (matches < 2)
{
    var ptr = password.Count - 1;
    while (true)
    {
        password[ptr]++;
        if (password[ptr] == 'i' || password[ptr] == 'o' || password[ptr] == 'l')
        {
            password[ptr]++;
        }

        if (password[ptr] == '{')
        {
            password[ptr] = 'a';
            ptr--;
            if (ptr == -1)
            {
                password.Insert(0, 'a');
                break;
            }
        }
        else
        {
            break;
        }
    }
    if (IsValid(password))
    {
        Console.WriteLine(string.Join("", password));
        matches++;
    }
}

static bool IsValid(List<char> password)
{
    var pairs = new HashSet<char>();
    var incr = false;
    for (var i = 0; i < password.Count; i++)
    {
        if (i > 0 && password[i] == password[i - 1])
        {
            pairs.Add(password[i]);
        }
        if (i > 1)
        {
            var c1 = password[i - 2] + 2;
            var c2 = password[i - 1] + 1;
            var c3 = password[i];
            incr = incr || (c1 == c2 && c1 == c3);
        }
    }

    return pairs.Count > 1 && incr;
}
