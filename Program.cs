using System.Text.RegularExpressions;

namespace adventofcode2023;

static class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Advent of code 2024!");
        await Day4();
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

    static Task Day3(bool enableInstructions = false)
    {
        Console.WriteLine("Welcome to the DAY 3!");
        Console.WriteLine("");
        Console.WriteLine("Started reading input...");
        var inputText = File.ReadAllText(@"input/day3.txt");
        var multiplicationResult = 0;
        var isEnabled = true;

        var multiplyMatches = Regex.Matches(inputText, "mul\\([0-9]+,[0-9]+\\)", RegexOptions.IgnoreCase);
        var doMatches = Regex.Matches(inputText, "do\\(\\)", RegexOptions.IgnoreCase);
        var dontMatches = Regex.Matches(inputText, "don't\\(\\)", RegexOptions.IgnoreCase);

        foreach (var match in multiplyMatches.ToList())
        {
            // Maybe cache this is possible?
            var dont = dontMatches.LastOrDefault(x => x.Index < match.Index);
            var @do = doMatches.LastOrDefault(x => x.Index < match.Index);

            // TODO: Optimize instructions logic
            if (dont == null)
                isEnabled = true;

            if (@do == null && dont != null && dont.Index < match.Index)
                isEnabled = false;

            if (@do != null && dont != null && dont.Index > @do.Index && dont.Index < match.Index)
                isEnabled = false;

            if (@do != null && dont != null && dont.Index < @do.Index && @do.Index < match.Index)
                isEnabled = true;

            if (isEnabled && enableInstructions)
            {
                var text = match.ToString();
                var indexOfComma = text.IndexOf(',');
                var indexOfBracket = text.IndexOf(')');
                var result = int.Parse(text.Substring(4, indexOfComma - 4)) * int.Parse(text.Substring(indexOfComma + 1, indexOfBracket - 1 - indexOfComma));
                multiplicationResult += result;
            }
        }

        Console.WriteLine($"Result of the multiplications: {multiplicationResult}");

        return Task.CompletedTask;
    }

    static Task Day4()
    {
        Console.WriteLine("Welcome to the DAY 4!");
        Console.WriteLine("");
        Console.WriteLine("Started reading input...");
        var inputText = File.ReadAllLines(@"input/day4.txt");
        var wordToFind = "XMAS";
        var totalOccurancesOfWord = 0;

        int rows = inputText.Length;
        int cols = inputText[0].Length;
        char[,] grid = new char[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            string currentString = inputText[i];
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = currentString[j];
            }
        }

        int wordLength = wordToFind.Length;

        int[,] directions = {
            {0, 1},   // right
            {1, 0},   // down
            {1, 1},   // down-right
            {1, -1},  // down-left
            {0, -1},  // left
            {-1, 0},  // up
            {-1, -1}, // up-left
            {-1, 1}   // up-right
        };

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                for (int direction = 0; direction < directions.GetLength(0); direction++)
                {
                    int newRow = row, newCol = col, k = 0;
                    while (k < wordLength)
                    {
                        if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols || grid[newRow, newCol] != wordToFind[k])
                            break;
                        newRow += directions[direction, 0];
                        newCol += directions[direction, 1];
                        k++;
                    }
                    if (k == wordLength)
                        totalOccurancesOfWord++;
                }
            }
        }

        Console.WriteLine($"{wordToFind} appears {totalOccurancesOfWord} times.");

        return Task.CompletedTask;
    }

}