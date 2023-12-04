

var input = File.ReadAllLines("Input.txt");

var one = PartOne(input);

int PartOne(string[] input)
{
    var parts = new List<Coord>();
    var numbers = new List<Number>();
    for (int y = 0; y < input.Length; y++)
    {
        for (int x = 0; x < input[y].Length; x++)
        {
            if (IsCharPart(input[y][x]))
                parts.Add(new Coord(x, y));

            if (int.TryParse(input[y][x].ToString(), out _))
            {
                var value = int.Parse(input[y].Substring(x, 3));
                numbers.Add(new Number(x, y, value));
                x += 2;
            }
        }
    }
    return 0;
}

bool IsCharPart(char v) => !int.TryParse(v.ToString(), out _) && v != '.';

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
struct Coord(int X, int Y);