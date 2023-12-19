

var input = File.ReadAllLines("Input.txt");

var one = PartOne(input);
Console.WriteLine(one);

var two = PartTwo(input);
Console.WriteLine(two);

int PartOne(string[] input)
{
    var historyLines = ParseHistory(input);

    var result = 0;
    foreach (var history in historyLines)
    {
        var extrapolationStart = new List<List<int>> { history };
        var extrapolation = Extrapolate(extrapolationStart);
        result += Predict(extrapolation);
    }

    return result;
}

int PartTwo(string[] input)
{
    var historyLines = ParseHistory(input);

    var result = 0;
    foreach (var history in historyLines)
    {
        var extrapolationStart = new List<List<int>> { history };
        var extrapolation = Extrapolate(extrapolationStart);
        result += Retrospect(extrapolation);
    }

    return result;
}

int Predict(List<List<int>> extrapolation)
{
    if (extrapolation.Count == 1) return extrapolation[0].Last();

    var current = extrapolation[0];
    extrapolation.RemoveAt(0);

    return current.Last() + Predict(extrapolation);
}

int Retrospect(List<List<int>> extrapolation)
{
    if (extrapolation.Count == 1) return extrapolation[0].First();

    var current = extrapolation[0];
    extrapolation.RemoveAt(0);

    return current.First() - Retrospect(extrapolation);
}

List<List<int>> Extrapolate(List<List<int>> extrapolation)
{
    var lastLine = extrapolation.Last().Select(x => x).ToList();

    if (lastLine.All(x => x == 0)) return extrapolation;

    lastLine.Reverse();
    var newLine = lastLine.Zip(lastLine.Skip(1), (x, y) => x - y).ToList();
    newLine.Reverse();
    extrapolation.Add(newLine);

    return Extrapolate(extrapolation);
}

List<List<int>> ParseHistory(string[] input)
{
    var history = new List<List<int>>();

    foreach (var line in input)
    {
        history.Add(line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(y => int.Parse(y)).ToList());
    }
    return history;
}