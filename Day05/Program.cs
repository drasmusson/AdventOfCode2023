
var input = File.ReadAllLines("Input.txt");

var one = PartOne(input);
Console.WriteLine(one);

long PartOne(string[] input)
{
    var seeds = ParseSeeds(input[0]);
    var maps = ParseMaps(input);

    var ends = new List<long>();
    foreach(var seed in seeds)
    {
        ends.Add(Traverse(seed, maps));
    }

    return ends.Min();
}

long Traverse(long seed, List<Map> maps)
{
    var source = seed;

    for (int i = 0; i < maps.Count; i++)
    {
        var map = maps[i];
        source = map.GetDestination(source);
    }

    return source;
}

List<Map> ParseMaps(string[] input)
{
    var maps = new List<Map>();
    var instructions = new List<Instruction>();
    for (int i = 3; i < input.Length; i++)
    {
        if (string.IsNullOrWhiteSpace(input[i])) continue;
        if (!int.TryParse(input[i][0].ToString(), out _))
        {
            maps.Add(new Map(instructions));
            instructions = new List<Instruction>();
            continue;
        }

        instructions.Add(new Instruction(input[i]));

    }

    maps.Add(new Map(instructions));
    return maps;
}

List<long> ParseSeeds(string input)
{
    var seeds = input.Split("seeds: ")[1].Split(" ").Select(x => long.Parse(x)).ToList();
    return seeds;
}

class Map
{
    List<Instruction> Instructions { get; }

    public Map(List<Instruction> instructions)
    {
        Instructions = instructions;
    }

    internal long GetDestination(long source)
    {
        var destination = source;

        foreach (var instruction in Instructions)
        {
            if (instruction.SourceRange.start > source || instruction.SourceRange.end < source)
                continue;

            var i = destination - instruction.SourceRange.start;
            destination = instruction.DestinationRange.start + i;
        }

        return destination;
    }
}

class Instruction
{
    public (long start, long end) DestinationRange { get; }
    public (long start, long end) SourceRange { get; }

    public Instruction(string parameters)
    {
        var numbers = parameters.Split(" ").Select(x => long.Parse(x)).ToList();
        DestinationRange = (numbers[0], numbers[0] + numbers[2] - 1);
        SourceRange = (numbers[1], numbers[1] + numbers[2] - 1);
    }
}