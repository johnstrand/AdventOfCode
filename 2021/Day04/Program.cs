using var reader = new StreamReader("input.txt");
var numbers = ReadNumbers(',');
var boards = new List<Board>();
while (!reader.EndOfStream)
{
    // Consume empty line
    reader.ReadLine();
    boards.Add(new Board());
    for (var i = 0; i < 5; i++)
    {
        boards[boards.Count - 1].LoadRow(ReadNumbers(' '));
    }
}

var first = true;
foreach (var number in numbers)
{
    foreach (var board in boards)
    {
        board.Play(number);
        if (board.HasWon() && first)
        {
            Console.WriteLine($"Part 1: {board.GetChecksum(number)}");
            first = false;
        }
        if (board.HasWon() && boards.All(b => b.HasWon()))
        {
            Console.WriteLine($"Part 2: {board.GetChecksum(number)}");
            break;
        }
    }
    if (boards.All(b => b.HasWon()))
    {
        break;
    }
}

List<int> ReadNumbers(char delim)
{
    return reader.ReadLine()!.Split(new[] { delim }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
}

public class Board
{
    private readonly List<int> state = new();
    private bool hasWon;

    public void LoadRow(IEnumerable<int> items)
    {
        state.AddRange(items);
    }

    public int GetChecksum(int number)
    {
        return state.Sum() * number;
    }

    public bool Play(int number)
    {
        var index = state.IndexOf(number);
        if (index == -1)
        {
            return false;
        }

        state[index] = 0;

        return true;
    }

    public bool HasWon()
    {
        if (hasWon)
        {
            return hasWon;
        }

        for (var i = 0; i < 5; i++)
        {
            if (Range(i, 5, 5).All(ix => state[ix] == 0))
            {
                return hasWon = true;
            }
            if (Range(i * 5, 5, 1).All(ix => state[ix] == 0))
            {
                return hasWon = true;
            }
        }

        return false;
    }

    private IEnumerable<int> Range(int start, int count, int step)
    {
        return Enumerable.Range(0, count).Select(n => start + n * step);
    }
}
