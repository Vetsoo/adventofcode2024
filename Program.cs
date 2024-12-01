namespace adventofcode2023;

static class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Advent of code 2024!");
        await Day1();
    }

    static async Task Day1()
    {
        Console.WriteLine("Welcome to the DAY 1!");
        Console.WriteLine("");
        Console.WriteLine("Started reading input...");
        var inputText = File.ReadLinesAsync(@"input/day1.txt");
        //var calibrationValues = inputText.Select(l => int.Parse($"{l.First(c => int.TryParse(c.ToString(), out int f))}{l.Last(c => int.TryParse(c.ToString(), out int f))}"));
    }
}