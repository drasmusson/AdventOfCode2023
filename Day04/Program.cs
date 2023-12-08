
using System.Text.RegularExpressions;

var input = File.ReadAllLines("Input.txt");

var one = PartOne(input);
Console.WriteLine(one);

var two = PartTwo(input);
Console.WriteLine(two);

int PartTwo(string[] input)
{
    var cards = ParseCards(input);

    var result = Play(cards);

    return result;
}

int Play(List<Card> cards)
{
    for (int i = cards.Count - 1; i >= 0; i--)
    {
        var currentCard = cards[i];

        var wins = currentCard.MyWinningNumbers();
        var newCopies = cards.GetRange(currentCard.CardNumber, wins);
        currentCard.AddedCopies += newCopies.Sum(x => x.AddedCopies) + 1;
    }

    return cards.Sum(x => x.AddedCopies);
}

int PartOne(string[] input)
{
    var cards = ParseCards(input);

    var score = 0;

    foreach (var card in cards)
    {
        var wins = card.MyWinningNumbers();
        if (wins > 0) score += CalculateScore(wins);
    }

    return score;
}

List<Card> ParseCards(string[] input)
{
    var cards = new List<Card>();
    var cardNumber = 1;
    foreach (var line in input)
    {
        var split = line.Split(": ")[1].Split(" | ");
        cards.Add(new Card(split, cardNumber));
        cardNumber++;
    }

    return cards;
}

int CalculateScore(int wins)
{
    int score = 1;
    
    for (int i = 1; i < wins; i++)
    {
        score *= 2;
    }
    return score;
}

class Card
{
    public int CardNumber { get; }
    public List<int> WinningNumbers { get; }
    public List<int> MyNumbers { get; }
    public int AddedCopies { get; set; }
    public Card(string[] parameters, int cardNumber)
    {
        var pattern = @"(\d+)";
        var winMatches = Regex.Matches(parameters[0], pattern);

        WinningNumbers = new List<int>();
        foreach (Match match in winMatches)
        {
            string number = match.Groups[1].Value;
            WinningNumbers.Add(int.Parse(number));
        }

        var myMatches = Regex.Matches(parameters[1], pattern);

        MyNumbers = new List<int>();
        foreach (Match match in myMatches)
        {
            string number = match.Groups[1].Value;
            MyNumbers.Add(int.Parse(number));
        }
        CardNumber = cardNumber;
    }

    internal int MyWinningNumbers()
    {
        var count = 0;

        foreach (var mNumber in MyNumbers)
            if (WinningNumbers.Any(x => x == mNumber))
                count++;

        return count;
    }
}