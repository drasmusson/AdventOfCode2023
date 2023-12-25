

var input = File.ReadAllLines("Input.txt");

var one = PartOne(input);
Console.WriteLine(one);

var two = PartTwo(input);
Console.WriteLine(two);

int PartTwo(string[] input)
{
    var map = ParseMap(input);
    var loop = Traverse(map);

    ReplaceStart(map);

    var inside = CountInside(map, loop);

    return inside;
}

int CountInside(Dictionary<Coord, Node> map, HashSet<Coord> loop)
{
    var inside = false;
    var insideCount = 0;
    var wallStart = ' ';
    for (int y = 0; y < map.Keys.Max(x => x.Y); y++)
        for (int x = 0; x < map.Keys.Max(x => x.X); x++)
        {
            var coord = new Coord(x, y);
            var node = map[coord];
            var isLoop = loop.Contains(coord);

            if (isLoop && node.Shape == '|') inside = !inside;
            if (isLoop && node.Shape == 'F')
            {
                inside = !inside;
                wallStart = 'F';
            }
            if (isLoop && node.Shape == 'L')
            {
                inside = !inside;
                wallStart = 'L';
            }
            if (isLoop && node.Shape == '7') inside = wallStart == 'F' ? !inside : inside;
            if (isLoop && node.Shape == 'J') inside = wallStart == 'F' ? inside : !inside;
            if (isLoop && node.Shape == '-') continue;

            if (!isLoop && inside) insideCount++;
        }
    return insideCount;
}

void ReplaceStart(Dictionary<Coord, Node> map)
{
    var startNode = map.First(x => x.Value.Shape == 'S').Value;

    if (map[startNode.Coord.BottNeighbor()].Neighbors().Contains(startNode.Coord) &&
        map[startNode.Coord.TopNeighbor()].Neighbors().Contains(startNode.Coord))
        startNode.Shape = '|';

    if (map[startNode.Coord.BottNeighbor()].Neighbors().Contains(startNode.Coord) &&
        map[startNode.Coord.LeftNeighbor()].Neighbors().Contains(startNode.Coord))
        startNode.Shape = '7';
    
    if (map[startNode.Coord.BottNeighbor()].Neighbors().Contains(startNode.Coord) &&
        map[startNode.Coord.RightNeighbor()].Neighbors().Contains(startNode.Coord))
        startNode.Shape = 'F';

    if (map[startNode.Coord.TopNeighbor()].Neighbors().Contains(startNode.Coord) &&
        map[startNode.Coord.RightNeighbor()].Neighbors().Contains(startNode.Coord))
        startNode.Shape = 'L';
    
    if (map[startNode.Coord.TopNeighbor()].Neighbors().Contains(startNode.Coord) &&
        map[startNode.Coord.LeftNeighbor()].Neighbors().Contains(startNode.Coord))
        startNode.Shape = 'J';

    if (map[startNode.Coord.RightNeighbor()].Neighbors().Contains(startNode.Coord) &&
        map[startNode.Coord.LeftNeighbor()].Neighbors().Contains(startNode.Coord))
        startNode.Shape = '-';
}

int PartOne(string[] input)
{
    var map = ParseMap(input);
    var loop = Traverse(map);
    var distance = GetFurthest(map, loop);
    return distance;
}

int GetFurthest(Dictionary<Coord, Node> map, HashSet<Coord> loop)
{
    var maxDistance = 0;
    foreach (var coord in loop)
    {
        if (map[coord].Distance > maxDistance)
            maxDistance = map[coord].Distance;
    }
    return maxDistance;
}

Dictionary<Coord, Node> ParseMap(string[] input)
{
    var map = new Dictionary<Coord, Node>();

    for (int y = 0; y < input.Count(); y++)
        for (int x = 0; x < input[y].Length; x++)
        {
            var coord = new Coord(x, y);
            map[coord] = new Node(input[y][x], coord);
        }

    return map;
}

HashSet<Coord> Traverse(Dictionary<Coord, Node> map)
{
    var queue = new Queue<Coord>();
    var visited = new HashSet<Coord>();
    var startC = map.First(x => x.Value.Shape == 'S').Key;
    queue.Enqueue(startC);
    visited.Add(startC);

    while (queue.Count > 0)
    {
        var current = queue.Dequeue();

        foreach (var neighbor in GetNeighbors(map, current))
        {
            if (!map.TryGetValue(neighbor, out var nNode)) continue;

            if (visited.Contains(neighbor)) 
            {
                if (nNode.Distance > map[current].Distance)
                    nNode.Distance = map[current].Distance + 1;

                continue;
            }

            nNode.Distance = map[current].Distance + 1;
            visited.Add(neighbor);
            queue.Enqueue(neighbor);
        }
    }
    return visited;
}

List<Coord> GetNeighbors(Dictionary<Coord, Node> map, Coord current)
{
    var cNode = map[current];
    var cNeighbors = cNode.Neighbors();
    var neighbors = new List<Coord>();

    foreach (var cNeighbor in cNeighbors)
    {

        if (map.ContainsKey(cNeighbor) && map[cNeighbor].Neighbors().Contains(current))
            neighbors.Add(cNeighbor);
    }
    return neighbors;
}

class Node
{
    public char Shape { get; set; }
    public Coord Coord { get; }
    public int Distance { get; set; }

    public Node(char shape, Coord coord)
    {
        Shape = shape;
        Coord = coord;
    }

    internal List<Coord> Neighbors()
    {
        var neighbors = new List<Coord>();
        switch (Shape)
        {
            case 'S':
                neighbors.AddRange(Coord.Neighbors());
                break;
            case '.':
                break;
            case '|':
                neighbors.Add(Coord.TopNeighbor());
                neighbors.Add(Coord.BottNeighbor());
                break;
            case '-':
                neighbors.Add(Coord.RightNeighbor());
                neighbors.Add(Coord.LeftNeighbor());
                break;
            case '7':
                neighbors.Add(Coord.LeftNeighbor());
                neighbors.Add(Coord.BottNeighbor());
                break;
            case 'J':
                neighbors.Add(Coord.TopNeighbor());
                neighbors.Add(Coord.LeftNeighbor());
                break;
            case 'L':
                neighbors.Add(Coord.TopNeighbor());
                neighbors.Add(Coord.RightNeighbor());
                break;
            case 'F':
                neighbors.Add(Coord.BottNeighbor());
                neighbors.Add(Coord.RightNeighbor());
                break;
            default:
                break;
        }
        return neighbors;
    }
}

record Coord(int X, int Y)
{
    internal IEnumerable<Coord> Neighbors()
    {
        yield return LeftNeighbor();
        yield return RightNeighbor();
        yield return TopNeighbor();
        yield return BottNeighbor();
    }

    internal Coord LeftNeighbor() => this with { X = X - 1 };
    internal Coord RightNeighbor() => this with { X = X + 1 };
    internal Coord TopNeighbor() => this with { Y = Y - 1 };
    internal Coord BottNeighbor() => this with { Y = Y + 1 };
}