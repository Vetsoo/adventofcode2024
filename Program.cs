using System.Text.RegularExpressions;

namespace adventofcode2023;

static class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Advent of code 2024!");
        await Day3();
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

    static Task Day2(bool activateDampner = false)
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
            if (!activateDampner && isNotSafe)
                continue;
            else if (activateDampner && isNotSafe)
            {
                // Brute forced, can be optimized probably
                for (var i = 0; i < levels.Length; i++)
                {
                    var newLevels = string.Join(" ", levels.Where((_, j) => j != i).ToArray()).Split(' ');
                    previousValue = int.Parse(newLevels[0]);
                    isNotSafe = false;
                    isDecreasing = false;
                    for (int g = 1; g < newLevels.Length; g++)
                    {
                        var value = int.Parse(newLevels[g]);
                        var diff = value - previousValue;

                        previousValue = value;

                        if (g == 1 && diff < 0)
                            isDecreasing = true;

                        if (diff == 0 || (diff < 0 && !isDecreasing) || (diff >= 0 && isDecreasing) || Math.Abs(diff) < minimumDiff || Math.Abs(diff) > maximumDiff)
                        {
                            isNotSafe = true;
                            break;
                        }
                        isNotSafe = false;
                    }

                    if (!isNotSafe)
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

    static Task Day3()
    {
        Console.WriteLine("Welcome to the DAY 3!");
        Console.WriteLine("");
        Console.WriteLine("Started reading input...");
        var inputText = File.ReadAllText(@"input/day3.txt");
        var multiplicationResult = 0;

        var matches = Regex.Matches(inputText, "mul\\([0-9]+,[0-9]+\\)", RegexOptions.IgnoreCase);

        foreach (var match in matches)
        {
            var text = match.ToString();
            var indexOfComma = text.IndexOf(',');
            var indexOfBracket = text.IndexOf(')');
            var result = int.Parse(text.Substring(4, indexOfComma - 4)) * int.Parse(text.Substring(indexOfComma + 1, indexOfBracket - 1 - indexOfComma));
            multiplicationResult += result;
        }

        Console.WriteLine($"Result of the multiplications: {multiplicationResult}");

        return Task.CompletedTask;
    }
}