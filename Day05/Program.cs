
var input = File.ReadAllLines("Input.txt");

var one = PartOne(input);
Console.WriteLine(one);

var two = PartTwo(input);
Console.WriteLine(two);

long PartTwo(string[] input)
{
    var seeds = ParseSeedRanges(input[0]);
    var maps = ParseMaps(input);

    var destinations = new List<long>();

    foreach (var seed in seeds)
    {
        destinations.Add(FindLowestDestination(seed, maps));
    }

    return destinations.Min();
}

long FindLowestDestination((long start, long end) seed, List<Map> maps)
{
    var destinations = new List<(long, long)> { seed };
    
    foreach (var map in maps)
    {
        var sources = new Queue<(long start, long end)>();
        foreach (var dest in destinations)
            sources.Enqueue(dest);

        destinations.Clear();
        
        while (sources.Count > 0)
        {
            var source = sources.Dequeue();
            (long source_start, long source_end) = source;
            var i = 0;
            for (i = 0; i < map.Instructions.Count; i++)
            {
                var instruction = map.Instructions[i];
                if (source.start >= instruction.SourceRange.start && source.end <= instruction.SourceRange.end)
                {
                    destinations.Add(instruction.GetDestination(source));
                    break;
                }
                if (source.end < instruction.SourceRange.start || source.start > instruction.SourceRange.end)
                    continue;
                if (source.start < instruction.SourceRange.start)
                {
                    sources.Enqueue(new (source.start, instruction.SourceRange.start - 1));
                    sources.Enqueue(new (instruction.SourceRange.start, source.end));
                    break;

                }
                if (source.end > instruction.SourceRange.end)
                {
                    sources.Enqueue(new (instruction.SourceRange.end + 1, source.end));
                    sources.Enqueue(new (source.start, instruction.SourceRange.end));
                    break;
                }
            }
            if (i == map.Instructions.Count)
                destinations.Add(source);
        }
    }
                
    var lowest = GetLowestLocation(destinations);
    return lowest;
}

long GetLowestLocation(List<(long start, long end)> locations)
{
    var lowest = locations[0].start;
    foreach (var location in locations)
        lowest = Math.Min(location.start, lowest);

    return lowest;
}

List<(long sourceStart, long sourceEnd)> GetOverlap((long start, long end) source, (long start, long end) map)
{
    var ranges = new List<(long start, long end)>();
    if (source.start > map.end || source.end < map.start) 
    {
        ranges.Add(source);
        return ranges;
    }
    if (source.start >= map.start && source.end <= map.end)
    {
        ranges.Add(source);
        return ranges;
    }
    if (source.start < map.start) 
    {
        ranges.Add((map.start, source.end));
        ranges.Add((source.start, map.start - 1));
    }
    if (source.end > map.end)
    {
        ranges.Add((source.start, map.end));
        ranges.Add((map.end + 1, source.end));
    }
    

    return ranges;
}

List<(long start, long end)> ParseSeedRanges(string input)
{
    var seedRanges = new List<(long start, long end)>();

    var numbers = input.Split("seeds: ")[1].Split(" ").Select(x => long.Parse(x)).ToList();
    for (int i = 0; i < numbers.Count - 1; i += 2)
    {
        if (i + 1 < numbers.Count)
        {
            var pair = (numbers[i], numbers[i] + numbers[i + 1] - 1);
            seedRanges.Add(pair);
        }
    }
    return seedRanges;
}


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
    public List<Instruction> Instructions { get; }

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

    internal (long start, long end) GetDestination((long start, long end) sourceRange)
    {
        if (sourceRange.end < SourceRange.start || sourceRange.start > SourceRange.end)
            return sourceRange;

        var i = DestinationRange.start - SourceRange.start;
        var destination = (sourceRange.start + i, sourceRange.end + i);

        return destination;
    }
}