var fishList = new List<List<int>>
{
    File.ReadAllText("input-test.txt").Split(',').Select(int.Parse).ToList()
};

var prev = 0;
for (var day = 0; day < 256; day++)
{
    var limit = fishList.Count;
    for (var schoolIndex = 0; schoolIndex < fishList.Count; schoolIndex++)
    {
        var fish = fishList[schoolIndex];
        var newSpawn = 0;
        for (var index = 0; index < fish.Count; index++)
        {
            fish[index]--;
            if (fish[index] == -1)
            {
                fish[index] = 6;
                newSpawn++;
            }
        }
        if (newSpawn > 0)
        {
            var newFish = Enumerable.Repeat(8, newSpawn).ToList();
            if (schoolIndex + 1 == fishList.Count)
            {
                fishList.Add(newFish);
            }
            else
            {
                fishList[schoolIndex + 1].AddRange(newFish);
            }
        }
    }

    var current = fishList.Sum(school => school.Count);
    Console.WriteLine($"Day {day + 1}: {current} (+{current - prev})");
    prev = current;
}

//Console.WriteLine(string.Join(", ", fish));
