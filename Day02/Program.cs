
var input = File.ReadAllLines("Input.txt");

var one = PartOne(input);
Console.WriteLine(one);

var two = PartTwo(input);
Console.WriteLine(two);

int PartOne(string[] input)
{
   var result = Solve(input); 

    return result;
}

int PartTwo(string[] input)
{
    var result = 0;
    foreach (var line in input)
    {
        result += GetMinPower(line);
    }
    return result;
}

int GetMinPower(string line)
{
    var result = 1;
    foreach (var rule in GetRules())
    {
        var cs = line.Split(" " + rule.color);
        result *= GetMinAmmount(rule.color, cs);
    }

    return result;
}

int GetMinAmmount(string color, string[] hands)
{
    var min = 1;
    for (int i = 0; i < hands.Length - 1; i++)
    {
        var num = int.Parse(hands[i].Substring(hands[i].LastIndexOf(" ") + 1));
        if (num > min) min = num;
    }
    return min;
}

int Solve(string[] input)
{
    var idSum = 0;
    foreach (var line in input)
    {
        if (IsGameValid(line))
            idSum += int.Parse(line.Split("Game ")[1].Split(":")[0]);

    }
    return idSum;
}

bool IsGameValid(string line)
{
    foreach (var rule in GetRules())
    {
        var cs = line.Split(" " + rule.color);
        if (!IsHandsValid(cs, rule)) return false;
    }

    return true;
}

bool IsHandsValid(string[] hands, Rule rule)
{
    for (int i = 0; i < hands.Length - 1; i++)
    {
        var numS = hands[i].Substring(hands[i].LastIndexOf(" ") + 1);
        if (int.Parse(numS) > rule.amount) return false;
    }
    return true;
}

List<Rule> GetRules() => new List<Rule> { new (12, "red" ), new (13, "green"), new (14, "blue") };
record Rule(int amount, string color);