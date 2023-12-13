
var input = File.ReadAllLines("Input.txt");

var one = PartOne(input);
Console.WriteLine(one);

int PartOne(string[] input)
{
    var hands = ParseHands(input);
    var ordered = hands.OrderByDescending(x => x.hand, new HandComparer<string>()).ToList();

    var result = CalculateResult(ordered);
    return result;
}

int CalculateResult(List<(string hand, int bet)> hands)
{
    var result = 0;
    for (int i = 0; i < hands.Count; i++)
        result += hands[i].bet * (i + 1);

    return result;
}

List<(string hand, int bet)> ParseHands(string[] input)
{
    var hands = new List<(string hand, int bet)>();

    foreach (var line in input)
    {
        var split = line.Split(" ");
        hands.Add((split[0], int.Parse(split[1])));
    }
    return hands;
}

class HandComparer<T> : IComparer<string>
{
    public HandComparer()
    {
    }

    public int Compare(string x, string y)
    {
        return customCompareFunc(x, y);
    }

    Func<string, string, int> customCompareFunc = (x, y) =>
    {
        // Implement your custom comparison logic here
        // Return a negative value if x should come before y,
        // a positive value if x should come after y,
        // or zero if they are considered equal.

        var xVal = HandValue(x);
        var yVal = HandValue(y);

        if (xVal > yVal) return -1;
        if (xVal < yVal) return 1;

        return CompareHighCard(x, y);
    };

    static int CompareHighCard(string x, string y)
    {
        for (int i = 0; i < x.Length; i++)
        {
            var xC = CardValue(x[i]);
            var yC = CardValue(y[i]);

            if (xC > yC) return -1;
            if (xC < yC) return 1;
        }
        return 0;
    }

    static int CardValue(char c)
    {
        switch (c)
        {
            case 'A':
                return 14;
            case 'K':
                return 13;
            case 'Q':
                return 12;
            case 'J':
                return 11;
            case 'T':
                return 10;
            default:
                return int.Parse(c.ToString());
        }
    }


    static int HandValue(string input)
    {
        var charFrequency = new Dictionary<char, int>();

        foreach (char c in input)
        {
            if (charFrequency.ContainsKey(c))
                charFrequency[c]++;
            else
                charFrequency[c] = 1;
        }
        var ordered = charFrequency.OrderByDescending(x => x.Value);

        var charCounts = ordered.ToList();
        switch (charCounts[0].Value)
        {
            case 5:
                return 6;
            case 4:
                return 5;
            case 3:
                if (charCounts[1].Value == 2)
                    return 4;
                else
                    return 3;
            case 2:
                if (charCounts[1].Value == 2)
                    return 2;
                else
                    return 1;
            default:
                return 0;
        }
    }
}