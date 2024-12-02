namespace adventofcode2023;

static class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Advent of code 2024!");
        await Day2();
    }

    static Task Day1()
    {
        Console.WriteLine("Welcome to the DAY 1!");
        Console.WriteLine("");
        Console.WriteLine("Started reading input...");
        var inputText = File.ReadAllLines(@"input/day1.txt");
        var left = new List<int>();
        var right = new List<int>();

        foreach (var line in inputText)
        {
            var splittedLine = line.Split("   ");
            _ = int.TryParse(splittedLine[0].ToString(), out int l);
            _ = int.TryParse(splittedLine[1].ToString(), out int r);
            left.Add(l);
            right.Add(r);
        }
        var orderLeft = left.OrderBy(x => x).ToList();
        var orderedRight = right.OrderBy(x => x).ToList();

        var sum = orderLeft.Select((x, i) => Math.Abs(x - orderedRight[i])).Sum();

        Console.WriteLine($"Sum of differences: {sum}");


        Console.WriteLine($"Calculating similarity score...");

        var similarityScore = 0;
        foreach (var id in left)
        {
            var multiplier = right.Count(x => x == id);
            similarityScore += id * multiplier;
        }

        Console.WriteLine($"Similarity score: {similarityScore}");

        return Task.CompletedTask;
    }

    static Task Day2()
    {
        Console.WriteLine("Welcome to the DAY 2!");
        Console.WriteLine("");
        Console.WriteLine("Started reading input...");
        var inputText = File.ReadAllLines(@"input/day2.txt");

        var safeReports = 0;
        var minimumDiff = 1;
        var maximumDiff = 3;
        foreach (var report in inputText)
        {
            var levels = report.Split(' ');
            var previousValue = int.Parse(levels[0]);
            var isNotSafe = false;
            var isDecreasing = false;
            for (int i = 1; i < levels.Length; i++)
            {
                var value = int.Parse(levels[i]);
                var diff = value - previousValue;

                previousValue = value;

                if (i == 1 && diff < 0)
                    isDecreasing = true;

                if (diff == 0 || (diff < 0 && !isDecreasing) || (diff >= 0 && isDecreasing) || Math.Abs(diff) < minimumDiff || Math.Abs(diff) > maximumDiff)
                {
                    isNotSafe = true;
                    break;
                }
            }
            if (isNotSafe)
                continue;

            safeReports++;
        }

        Console.WriteLine($"Amount of safe reports: {safeReports}");

        return Task.CompletedTask;
    }
}