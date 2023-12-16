
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

var input = File.ReadAllLines("Input.txt");

// var one = PartOne(input);
// Console.WriteLine(one);

var two = PartTwo(input);
Console.WriteLine(two);

long PartTwo(string[] input)
{
    var instructions = input[0];
    var map = ParseMap(input[2..]);

    var steps = StepsToEnds(instructions, map);
    return steps;
}

long StepsToEnds(string instructions, Dictionary<string, (string l, string r)> map)
{
    var starts = GetStarts(map);
    var cycles = new List<(int start, int cycle)>();
    foreach (var start in starts)
    {
        cycles.Add(FindCycle(instructions, map, start));
    }
    
    var startLength = (long)cycles[0].start;
    var cycleLength = (long)cycles[0].cycle;
    foreach (var compareCycle in cycles[1..])
    {
        while (startLength < compareCycle.start || (startLength - compareCycle.start) % compareCycle.cycle != 0)
            startLength += cycleLength;

        long currentFullCycle = startLength + cycleLength;
        while ((currentFullCycle - compareCycle.start) % compareCycle.cycle != 0)
            currentFullCycle += cycleLength;
        cycleLength = currentFullCycle - startLength;

    }
    return startLength;
}

(int start, int cycle) FindCycle(string instructions, Dictionary<string, (string l, string r)> map, (string l, string r) start)
{
    (int start, int cycle) travel = (0, 0);

    var starts = new Queue<(string l, string r)>();
    starts.Enqueue(start);

    var i = 0;
    while(true)
    {
        var current = starts.Dequeue();
        var instruction = instructions[i % instructions.Length];

        var next = instruction == 'L' ? current.l : current.r;

        if (IsEnd(next))
        {
            if (travel.start == 0) travel.start = i + 1;
            else
            {
                var cycle = i - travel.start + 1;
                travel.cycle = cycle;
                return travel;
            }
        }
        starts.Enqueue(map[next]);

        i++;
    }
}

bool IsEnd(string node) => node[2] == 'Z';

Queue<(string l, string r)> GetStarts(Dictionary<string, (string l, string r)> map)
{
    var queue = new Queue<(string l, string r)>();
    foreach (var item in map)
    {
        if (item.Key[2] == 'A')
            queue.Enqueue(item.Value);
    }
    return queue;
}

int PartOne(string[] input)
{
    var instructions = input[0];
    var map = ParseMap(input[2..]);

    var steps = StepsToZZZ(instructions, map);

    return steps;
}

int StepsToZZZ(string instructions, Dictionary<string, (string l, string r)> map)
{
    var element = map["AAA"];
    var i = 0;
    while(true)
    {
        var instruction = instructions[i % instructions.Length];

        if (instruction == 'L')
        {
            if (element.l == "ZZZ") 
                return i + 1;
            element = map[element.l];
        }

        if (instruction == 'R')
        {
            if (element.r == "ZZZ")
                return i + 1;
            element = map[element.r];
        }

        i++;
    }
}

Dictionary<string, (string l, string r)> ParseMap(string[] strings)
{
    var map = new Dictionary<string, (string l, string r)>();
    foreach (var line in strings)
    {
        var key = line.Split(" = ")[0];
        var lrSplit = line.Split(" = ")[1].Split(", ");
        var lr = (lrSplit[0][1..], string.Concat(lrSplit[1].Take(3)));

        map[key] = lr;
    }
    return map;
}