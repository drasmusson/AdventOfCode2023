


var input = File.ReadAllLines("Input.txt");

var one = PartOne(input);
Console.WriteLine(one);

var two = PartTwo(input);

int PartTwo(string[] input)
{
    var hands = ParseHands(input);
    var ordered = hands.OrderByDescending(x => x.hand, new HandComparer2<string>()).ToList();

    var result = CalculateResult(ordered);
    return result;
}

Console.WriteLine(two);

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
    public int Compare(string x, string y)
    {
        var xVal = HandValue(x);
        var yVal = HandValue(y);

        if (xVal > yVal) return -1;
        if (xVal < yVal) return 1;

        return CompareHighCard(x, y);
    }

    int CompareHighCard(string x, string y)
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

    int CardValue(char c)
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


    int HandValue(string input)
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

class HandComparer2<T> : IComparer<string>
{
    public int Compare(string x, string y)
    {
        var xJ = x.Contains("J") ? ConvertJ(x) : x;
        var yJ = y.Contains("J") ? ConvertJ(y) : y;

        var xVal = HandValue(xJ);
        var yVal = HandValue(yJ);

        if (xVal > yVal) return -1;
        if (xVal < yVal) return 1;

        return CompareHighCard(x, y);
    }

    private string ConvertJ(string input)
    {
        if (input == "JJJJJ") return input;

        var charCounts = CountCards(input);

        if (charCounts[0].Value >= 3 && charCounts[0].Key != 'J')
        {
            return input.Replace('J', charCounts[0].Key);
        }
        // two pairs, none having in jokers
        if (charCounts[0].Value > 1 && charCounts.First(x => x.Key == 'J').Value < 2)
        {
            var c = charCounts[0].Value > charCounts[1].Value ? charCounts[0].Key : charCounts[1].Key;
            return input.Replace('J', c);
        }
        // two pairs, one of them being in jokers
        if (charCounts[0].Value > 1 && charCounts.First(x => x.Key == 'J').Value > 1)
        {
            var c = charCounts[0].Key == 'J' ? charCounts[1].Key : charCounts[0].Key;
            return input.Replace('J', c);
        }

        var hC = ' ';
        var max = 0;
        for (int i = 0; i < input.Length; i++)
        {
            var cC = input[i];
            var cV = CardValue(cC);
            if (cV > max)
            {
                hC = cC;
                max = cV;
            }
        }
        return input.Replace('J', hC);
    }

    static List<KeyValuePair<char, int>> CountCards(string input)
    {
        var charFrequency = new Dictionary<char, int>();

        foreach (char c in input)
        {
            if (charFrequency.ContainsKey(c))
                charFrequency[c]++;
            else
                charFrequency[c] = 1;
        }
        var ordered = charFrequency.OrderByDescending(x => x.Value).ToList();
        return ordered;
    }

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
                return 1;
            case 'T':
                return 10;
            default:
                return int.Parse(c.ToString());
        }
    }


    static int HandValue(string input)
    {
        var ordered = CountCards(input);

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
