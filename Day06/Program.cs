
using System.Text.RegularExpressions;

var input = File.ReadAllLines("Input.txt");

var one = PartOne(input);
Console.WriteLine(one);

var two = PartTwo(input);
Console.WriteLine(two);

long PartTwo(string[] input)
{
    var leaderBoard = ParseLeaderBoard(input);
    var winningPlays = CountWinningPlays(leaderBoard);

    return winningPlays;
}

long CountWinningPlays((long time, long distance) leaderBoard)
{
    var lossesFromStart = GetLosses(leaderBoard, 0, x => x + 1);
    var lossesFromEnd = GetLosses(leaderBoard, leaderBoard.time, x => x - 1);

    return leaderBoard.time - lossesFromStart - lossesFromEnd + 1;
}

long GetLosses((long time, long distance) leaderBoard, long start, Func<long, long> increment)
{
    var notWinning = 0L;

    var queue = new Queue<long>();
    queue.Enqueue(start);

    var timeRemaining = leaderBoard.time;

    while (true)
    {
        var charge = queue.Dequeue();

        if (PlayIsWinning(leaderBoard.distance, charge, leaderBoard.time - charge))
            break;
        
        queue.Enqueue(increment(charge));
        timeRemaining--;
        notWinning++;
    }

    return notWinning;
}

int PartOne(string[] input)
{
    var leaderBoards = new List<(int time, int distance)>();
    leaderBoards = ParseLeaderBoards(input);
    var winningPlays = new List<int>();
    foreach (var leaderBoard in leaderBoards)
    {
        winningPlays.Add(SearchForWinningPlays(leaderBoard));
    }
    var answer = winningPlays.Aggregate((x, y) => x * y);

    return answer;
}

int SearchForWinningPlays((int time, int distance) leaderBoard)
{
    var winningPlays = 0;

    var queue = new Queue<int>();
    queue.Enqueue(0);

    var timeRemaining = leaderBoard.time;
    while (timeRemaining > 0)
    {
        var charge = queue.Dequeue();

        if (PlayIsWinning(leaderBoard.distance, charge, leaderBoard.time - charge))
            winningPlays++;
        
        queue.Enqueue(charge + 1);
        timeRemaining--;
    }
    return winningPlays;
}

bool PlayIsWinning(long distToBeat, long play, long remaining) => play * remaining > distToBeat;

List<(int time, int distance)> ParseLeaderBoards(string[] input)
{
    var pattern = @"(\d+)";
    var times = Regex.Matches(input[0], pattern);
    var distances = Regex.Matches(input[1], pattern);

    var leaderBoard = new List<(int, int)>();
    for (int i = 0; i < times.Count; i++)
        leaderBoard.Add((int.Parse(times[i].Value), int.Parse(distances[i].Value)));

    return leaderBoard;
}

(long time, long distance) ParseLeaderBoard(string[] input)
{
    var time = long.Parse(input[0].Split("Time:")[1].Replace(" ", String.Empty));
    var distance = long.Parse(input[1].Split("Distance:")[1].Replace(" ", String.Empty));

    return (time, distance);
}