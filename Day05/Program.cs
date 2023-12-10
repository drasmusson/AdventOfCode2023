
var input = File.ReadAllLines("Input.txt");

// var one = PartOne(input);
// Console.WriteLine(one);

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
    var sources = new Queue<(long start, long end)>();
    sources.Enqueue(seed);
    var destinations = new List<(long start, long end)>();
    foreach (var map in maps)
    {
        foreach (var destination in destinations)
            sources.Enqueue(destination);
        
        destinations.Clear();

        while (sources.Count > 0)
        {
            var source = sources.Dequeue();
            foreach (var instruction in map.Instructions)
            {
                var overlap = GetOverlap(source, instruction.SourceRange);
                var destinationRange = instruction.GetDestination(overlap);
                destinations.Add(destinationRange);
            }
        }
    }
    var lowest = destinations.Min(x => x.start);
    return lowest;
}

(long sourceStart, long sourceEnd) GetOverlap((long start, long end) seedRange, (long start, long end) sourceRange)
{
    if (seedRange.start > sourceRange.end) return seedRange;
    if (seedRange.end < sourceRange.start) return seedRange;
    // if (seedRange.start >= sourceRange.start && seedRange.start <= seedRange.end) return ()
    var overlapStart = Math.Max(seedRange.start, sourceRange.start);
    var overlapEnd = Math.Min(seedRange.end, sourceRange.end);
    if (overlapStart <= overlapEnd) return (overlapStart, overlapEnd);

    return seedRange;
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
    private long Length { get; }

    public Instruction(string parameters)
    {
        var numbers = parameters.Split(" ").Select(x => long.Parse(x)).ToList();
        DestinationRange = (numbers[0], numbers[0] + numbers[2] - 1);
        SourceRange = (numbers[1], numbers[1] + numbers[2] - 1);
        Length = numbers[2];
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