

var input = File.ReadAllLines("Input.txt");

var one = PartOne(input);
Console.WriteLine(one);

var two = PartTwo(input);
Console.WriteLine(two);

int PartOne(string[] input)
{
    var parts = new List<Coord>();
    var numbers = new List<Number>();
    for (int y = 0; y < input.Length; y++)
    {
        for (int x = 0; x < input[y].Length; x++)
        {
            if (IsPartSymbol(input[y][x]))
                parts.Add(new Coord(x, y));

            if (int.TryParse(input[y][x].ToString(), out _))
            {
                var endIndex = x;
                while(endIndex < input[y].Length && char.IsDigit(input[y][endIndex])) endIndex++;

                var numberLength = endIndex - x;
                var value = int.Parse(input[y].Substring(x, numberLength));
                numbers.Add(new Number(x, y, value));
                x += numberLength - 1;
            }
        }
    }

    foreach (var part in parts)
    {
        foreach (var coord in part.GetNeighbours())
        {
            foreach (var number in numbers)
            {
                if (number.Coords.Any(x => x == coord)) number.Touched = true;
            }
        }
    }

    var result = numbers.Where(x => x.Touched).Sum(y => y.Value);

    return result;
}

int PartTwo(string[] input)
{
    var gearCandidates = new List<Coord>();
    var numbers = new List<Number>();
    for (int y = 0; y < input.Length; y++)
    {
        for (int x = 0; x < input[y].Length; x++)
        {
            if (IsGearSymbol(input[y][x]))
                gearCandidates.Add(new Coord(x, y));

            if (int.TryParse(input[y][x].ToString(), out _))
            {
                var endIndex = x;
                while(endIndex < input[y].Length && char.IsDigit(input[y][endIndex])) endIndex++;

                var numberLength = endIndex - x;
                var value = int.Parse(input[y].Substring(x, numberLength));
                numbers.Add(new Number(x, y, value));
                x += numberLength - 1;
            }
        }
    }

    var gearRatio = 0;

    foreach (var gearCandidate in gearCandidates)
    {
        foreach (var coord in gearCandidate.GetNeighbours())
        {
            foreach (var number in numbers)
            {
                if (number.Coords.Any(x => x == coord)) number.Touched = true;
            }
        }
        if (numbers.Count(x => x.Touched) == 2)
        {
            var ratio = numbers.First(x => x.Touched).Value * numbers.Last(x => x.Touched).Value;
            gearRatio += ratio;
        }
        foreach (var number in numbers) number.Touched = false;
    }

    return gearRatio;
}

bool IsPartSymbol(char v) => !int.TryParse(v.ToString(), out _) && v != '.';
bool IsGearSymbol(char v) => v == '*';

record Number
{
    public Number(int x, int y, int value)
    {
        Coords = new List<Coord>{ new Coord(x, y), new Coord(x + 1, y), new Coord(x + 2, y) };
        Value = value;
    }
    public List<Coord> Coords { get; }
    public int Value { get; set; }
    public bool Touched { get; set; }
}
record Coord(int X, int Y)
{
    internal IEnumerable<Coord> GetNeighbours()
    {
        yield return this with { X = X - 1, Y = Y - 1 };
        yield return this with { Y = Y - 1 };
        yield return this with { X = X + 1, Y = Y - 1 };
        yield return this with { X = X - 1 };
        yield return this with { X = X + 1 };
        yield return this with { X = X - 1, Y = Y + 1 };
        yield return this with { Y = Y + 1 };
        yield return this with { X = X + 1, Y = Y + 1 };
    }
}