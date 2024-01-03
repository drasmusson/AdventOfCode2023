
var input = File.ReadAllLines("Input.txt");

var one = PartOne(input);
Console.WriteLine(one);

var two = PartTwo(input);
Console.WriteLine(two);

long PartTwo(string[] input)
{
    var expands = GetExpandSpaces(input);
    var map = ParseMap(input.ToList());
    var pairs = GeneratePairs(map);
    var distances = FindDistances(map, expands, pairs, 1000000);

    return distances.Sum();
}

long PartOne(string[] inputAr)
{
    var expands = GetExpandSpaces(input);
    var map = ParseMap(input.ToList());
    var pairs = GeneratePairs(map);
    var distances = FindDistances(map, expands, pairs, 2);

    return distances.Sum();
}

List<long> FindDistances(Dictionary<Coord, int> map, (List<int> x, List<int> y) expands, List<(int s, int e)> pairs, int expandSize)
{
    var distances = new List<long>();
    foreach (var pair in pairs)
        distances.Add(Distance(map, expands, pair.s, pair.e, expandSize));
    
    return distances;
}

long Distance(Dictionary<Coord, int> map, (List<int> x, List<int> y) expands, int s, int e, int expandSize)
{
    var start = map.First(x => x.Value == s).Key;
    var end = map.First(x => x.Value == e).Key;

    var xD = Math.Abs(end.X - start.X);
    var yD = Math.Abs(end.Y - start.Y);

    var xAdd = 0;
    var xInterval = start.X < end.X ? (start.X, end.X) : (end.X, start.X);
    foreach (var xExpand in expands.x)
    {
        if (xExpand >= xInterval.Item1 && xExpand <= xInterval.Item2) xAdd += expandSize - 1;
    }
    var yAdd = 0;
    var yInterval = start.Y < end.Y ? (start.Y, end.Y) : (end.Y, start.Y);
    foreach (var yExpand in expands.y)
    {
        if (yExpand >= yInterval.Item1 && yExpand <= yInterval.Item2) yAdd += expandSize - 1;
    }

    return xD + xAdd + yD + yAdd;
}

List<(int s, int e)> GeneratePairs(Dictionary<Coord, int> map)
{
    var idRoof = map.Values.Max();
    var pairs = new List<(int s, int e)>();

    for (int i = 1; i <= idRoof; i++)
        for (int j = i + 1; j <= idRoof; j++)
            pairs.Add((i, j));

    return pairs;
}

Dictionary<Coord, int> ParseMap(List<string> input)
{
    var map = new Dictionary<Coord, int>();
    var uniId = 1;

    for (int y = 0; y < input.Count(); y++)
        for (int x = 0; x < input[y].Length; x++)
        {
            var coord = new Coord(x, y);
            if (input[y][x] == '#')
            {
                map[coord] = uniId;
                uniId++;
                continue;
            }
            map[coord] = 0;
        }

    return map;
}

(List<int> xExpands, List<int> yExpands) GetExpandSpaces(string[] input)
{
    var xExpands = new List<int>();
    var yExpands = new List<int>();

    for (int i = 0; i < input.Count(); i++)
    {
        if (input[i].All(c => c == '.'))
        {
            yExpands.Add(i);
        }
    }
    for (int i = 0; i < input[0].Length; i++)
    {
        if (input.All(line => line[i] == '.'))
        {
            xExpands.Add(i);
        }
    }
    return (xExpands, yExpands);
}

record Coord(int X, int Y);