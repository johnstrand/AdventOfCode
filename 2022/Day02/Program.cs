/*
    A - Rock
    B - Paper
    C - Scissors

    X - Rock
    Y - Paper
    Z - Scissors

    X - Lose
    Y - Draw
    Z - Win
*/

// Choices where, given an opponents move, which move the player should use to win
var winningMoves = new HashSet<(int opponent, int player)>
{
    (1, 2), // Rock and Paper
    (2, 3), // Paper and Scissors
    (3, 1) // Scissors and Rock
};

var score = 0;
var score2 = 0;

foreach (var line in File.ReadAllLines("input.txt"))
{
    var op_val = line[0] - 'A' + 1;
    var pl_val = line[2] - 'X' + 1;

    score += pl_val + (op_val == pl_val ? 3 : winningMoves.Contains((op_val, pl_val)) ? 6 : 0);

    if (pl_val == 1) // Lose
    {
        // Find the reverse of the winning move
        var move = winningMoves.First(i => i.player == op_val).opponent;
        score2 += move;
    }
    else if (pl_val == 2) // Draw
    {
        score2 += 3 + op_val;
    }
    else // Win
    {
        var move = winningMoves.First(i => i.opponent == op_val).player;
        score2 += 6 + move;
    }
}

Console.WriteLine($"Part 1: {score}");
Console.WriteLine($"Part 2: {score2}");