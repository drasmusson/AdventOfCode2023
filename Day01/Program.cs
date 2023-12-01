
var input = File.ReadAllLines("Input.txt");
var partOne = PartOne(input);
Console.WriteLine(partOne);

var partTwo = PartTwo(input);
Console.WriteLine(partTwo);

int PartOne(string[] input)
{
	var result = 0;
	foreach (var line in input)
	{
        var firstNum = GetFirstNum(line);
		var charArray = line.ToCharArray();
		Array.Reverse(charArray);
        var secNum = GetFirstNum(new string(charArray));
		var num = firstNum + secNum;
		result += int.Parse(num);
	}

	return result;
}

int PartTwo(string[] input)
{
	var result = 0;

	foreach(var line in input)
	{
		var numbers = GetNumbers();
		var firstIndex = line.Length;
		var firstNum = "";
		var lastIndex = -1;
		var secNum = "";
		foreach (var number in numbers)
		{
			var fI = line.IndexOf(number);
			var lI = line.LastIndexOf(number);
			if (fI < firstIndex && fI != -1)
			{
				firstNum = number;
				firstIndex = fI;
			}
			if (lI > lastIndex)
			{
				secNum = number;
				lastIndex = lI;
			}
		}
		var num = GetNum(firstNum) + GetNum(secNum);
		result += int.Parse(num);
	}
	return result;
}

string GetNum(string str) => str switch
{
    "one" or "1" => "1",
    "two" or "2" => "2",
    "three" or "3" => "3",
    "four" or "4" => "4",
    "five" or "5" => "5",
    "six" or "6" => "6",
    "seven" or "7" => "7",
    "eight" or "8" => "8",
    "nine" or "9" => "9",
    _ => throw new NotImplementedException()
}; 

List<string> GetNumbers() => new List<string> { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

string GetFirstNum(string line) 
{
    for (int i = 0; i < line.Length; i++)
		if (int.TryParse(line[i].ToString(), out var n)) return line[i].ToString();
	return "";
}