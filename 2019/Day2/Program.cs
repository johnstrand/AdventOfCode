// Even more cleaning
List<int> prg = null;

Dictionary<int, Action<int, int, int>> codes = new()
{
    {
        1,
        new Action<int, int, int>((ptrIn1, ptrIn2, ptrOut) => prg[ptrOut] = prg[ptrIn1] + prg[ptrIn2])
    },
    {
        2,
        new Action<int, int, int>((ptrIn1, ptrIn2, ptrOut) => prg[ptrOut] = prg[ptrIn1] * prg[ptrIn2])
    }
};

var source = File.ReadAllText("input.txt").Split(',').Select(int.Parse).ToList();
var part2 = true;
var next = VerbNounFactory();
do
{
    prg = source.ToList();
    if (part2)
    {
        var (verb, noun) = next(true);
        prg[1] = noun;
        prg[2] = verb;
    }
    var ptr = 0;
    while (prg[ptr] != 99)
    {
        codes[prg[ptr]](prg[ptr + 1], prg[ptr + 2], prg[ptr + 3]);
        ptr += 4;
    }
}
while (part2 && prg[0] != 19690720);
Console.WriteLine(prg[0]);
var (v, n) = next(false);
Console.WriteLine($"Noun: {n}. Verb: {v}");

Func<bool, (int verb, int noun)> VerbNounFactory()
{
    var currentVerb = -1;
    var currentNoun = 0;
    return incr =>
    {
        if (incr)
        {
            currentVerb++;
            if (currentVerb == 100)
            {
                currentNoun++;
                currentVerb = 0;
            }
        }

        return (currentVerb, currentNoun);
    };
}