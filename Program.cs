using System.Text.RegularExpressions;

namespace adventofcode2023;

static class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Advent of code 2024!");
        await Day6();
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

    static Task Day4(bool xMas = false)
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
        if (!xMas)
        {
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
        }
        else
        {
            int totalOccurancesOfWordInXShape = 0;

            for (int row = 1; row < rows - 1; row++)
            {
                for (int col = 1; col < cols - 1; col++)
                {
                    // Check for all possible "X-MAS" patterns --> this is not a good approach, but it works lol
                    if (
                        // "MAS" pattern in topLeft to bottomRight
                        (grid[row - 1, col - 1] == 'M' && grid[row, col] == 'A' && grid[row + 1, col + 1] == 'S' &&
                         grid[row - 1, col + 1] == 'M' && grid[row + 1, col - 1] == 'S') ||

                        // "MAS" pattern in topLeft to bottomRight & reverse
                        (grid[row - 1, col - 1] == 'M' && grid[row, col] == 'A' && grid[row + 1, col + 1] == 'S' &&
                         grid[row - 1, col + 1] == 'S' && grid[row + 1, col - 1] == 'M') ||

                        // "SAM" in topLeft to bottomRight (reverse)
                        (grid[row - 1, col - 1] == 'S' && grid[row, col] == 'A' && grid[row + 1, col + 1] == 'M' &&
                         grid[row - 1, col + 1] == 'S' && grid[row + 1, col - 1] == 'M') ||

                        // "SAM" in topLeft to bottomRight (reverse) & normal
                        (grid[row - 1, col - 1] == 'S' && grid[row, col] == 'A' && grid[row + 1, col + 1] == 'M' &&
                         grid[row - 1, col + 1] == 'M' && grid[row + 1, col - 1] == 'S') ||

                        // "MAS" pattern in topRight to bottomLeft
                        (grid[row - 1, col + 1] == 'M' && grid[row, col] == 'A' && grid[row + 1, col - 1] == 'S' &&
                         grid[row - 1, col - 1] == 'M' && grid[row + 1, col + 1] == 'S') ||

                        // "MAS" pattern in topRight to bottomLeft & reverse
                        (grid[row - 1, col + 1] == 'M' && grid[row, col] == 'A' && grid[row + 1, col - 1] == 'S' &&
                         grid[row - 1, col - 1] == 'S' && grid[row + 1, col + 1] == 'M') ||

                        // "SAM" in topRight to bottomLeft (reverse) 
                        (grid[row - 1, col + 1] == 'S' && grid[row, col] == 'A' && grid[row + 1, col - 1] == 'M' &&
                         grid[row - 1, col - 1] == 'S' && grid[row + 1, col + 1] == 'M') ||

                        // "SAM" in topRight to bottomLeft (reverse) & normal
                        (grid[row - 1, col + 1] == 'S' && grid[row, col] == 'A' && grid[row + 1, col - 1] == 'M' &&
                         grid[row - 1, col - 1] == 'M' && grid[row + 1, col + 1] == 'S')
                    )
                    {
                        totalOccurancesOfWordInXShape++;
                    }
                }
            }

            Console.WriteLine($"{wordToFind} appears {totalOccurancesOfWordInXShape} times in X shape.");
        }


        return Task.CompletedTask;
    }

    static Task Day5()
    {
        Console.WriteLine("Welcome to the DAY 5!");
        Console.WriteLine("");
        Console.WriteLine("Started reading input...");
        var inputText = File.ReadAllLines(@"input/day5.txt");
        var instructions = new List<Tuple<int, int>>();
        var updates = new List<List<int>>();
        var loadInstructions = true;
        foreach (var line in inputText)
        {
            if (loadInstructions)
            {
                if (line == string.Empty)
                {
                    loadInstructions = false;
                    continue;
                }
                var splittedLine = line.Split('|');
                var tuple = new Tuple<int, int>(int.Parse(splittedLine[0]), int.Parse(splittedLine[1]));
                instructions.Add(tuple);
            }
            else
            {
                var splittedLine = line.Split(',').Select(x => int.Parse(x)).ToList();
                updates.Add(splittedLine);
            }
        }

        var linesWithCorrectOrder = new List<List<int>>();
        var linesWithIncorrectOrder = new Dictionary<int, List<int>>();
        for (int i = 0; i < updates.Count; i++)
        {
            var sequence = updates[i];
            var isCorrectlyOrdered = true;
            isCorrectlyOrdered = CheckOrder(instructions, updates, linesWithIncorrectOrder, i, sequence, isCorrectlyOrdered);

            if (isCorrectlyOrdered)
                linesWithCorrectOrder.Add(sequence);
        }

        var sumOfMiddleNumbersOfCorrectlyOrderedUpdates = linesWithCorrectOrder.Select(x => x[(x.Count - 1) / 2]).Sum();
        var sumOfMiddleNumbersOfIncorrectlyOrderedUpdates = linesWithIncorrectOrder.Select(x => x.Value[(x.Value.Count - 1) / 2]).Sum();

        Console.WriteLine($"Sum of the middle page numbers from the correctly-ordered updates: {sumOfMiddleNumbersOfCorrectlyOrderedUpdates}");
        Console.WriteLine($"Sum of the middle page numbers from the incorrectly-ordered updates: {sumOfMiddleNumbersOfIncorrectlyOrderedUpdates}");

        return Task.CompletedTask;

        static bool CheckOrder(List<Tuple<int, int>> instructions, List<List<int>> updates, Dictionary<int, List<int>> linesWithIncorrectOrder, int i, List<int> sequence, bool isCorrectlyOrdered)
        {
            for (int j = 0; j < updates[i].Count; j++)
            {
                var pageNumber = sequence[j];
                var beforeNumbers = instructions.Where(x => x.Item2 == pageNumber).Select(x => x.Item1);
                var afterNumbers = instructions.Where(x => x.Item1 == pageNumber).Select(x => x.Item2);

                foreach (var beforeNumber in beforeNumbers)
                {
                    var indexOfNumber = sequence.IndexOf(beforeNumber);

                    if (indexOfNumber == -1)
                        continue;

                    if (indexOfNumber > j)
                    {
                        sequence.RemoveAt(indexOfNumber);
                        var indexToInsertAt = j;
                        if (j == 0)
                            indexToInsertAt = 0;
                        sequence.Insert(indexToInsertAt, beforeNumber);
                        isCorrectlyOrdered = false;
                        CheckOrder(instructions, updates, linesWithIncorrectOrder, i, sequence, true);
                        break;
                    }
                }

                foreach (var afterNumber in afterNumbers)
                {
                    var indexOfNumber = sequence.IndexOf(afterNumber);

                    if (indexOfNumber == -1)
                        continue;

                    if (indexOfNumber < j)
                    {
                        sequence.RemoveAt(indexOfNumber);
                        var indexToInsertAt = j + 1;
                        if (j == sequence.Count - 1)
                            indexToInsertAt = sequence.Count - 1;
                        sequence.Insert(indexToInsertAt, afterNumber);
                        isCorrectlyOrdered = false;
                        CheckOrder(instructions, updates, linesWithIncorrectOrder, i, sequence, true);
                        break;
                    }
                }

                if (!isCorrectlyOrdered)
                {
                    linesWithIncorrectOrder[i] = sequence;
                    break;
                }
            }

            return isCorrectlyOrdered;
        }
    }

    static Task Day6()
    {
        Console.WriteLine("Welcome to the DAY 6!");
        Console.WriteLine("");
        Console.WriteLine("Started reading input...");
        var inputText = File.ReadAllLines(@"input/day6.txt");
        var obstruction = '#';
        var markers = new Dictionary<int, char>{
            {0, '>'},
            {1, 'v'},
            {2, '<'},
            {3, '^'},
        };

        int[,] directions = {
            {0, 1},   // right
            {1, 0},   // down
            {0, -1},  // left
            {-1, 0},  // up
        };

        int rows = inputText.Length;
        int cols = inputText[0].Length;
        char[,] grid = new char[rows, cols];
        var markerPosition = new Tuple<int, int, int>(0, 0, 0);

        for (int i = 0; i < rows; i++)
        {
            string currentString = inputText[i];
            for (int j = 0; j < cols; j++)
            {
                var markerValue = markers.FirstOrDefault(x => x.Value == currentString[j]);
                if (markerValue.Key != 0)
                    markerPosition = new Tuple<int, int, int>(markerValue.Key, i, j);
                grid[i, j] = currentString[j];
            }
        }

        var uniquePositions = new List<Tuple<int, int>> { new Tuple<int, int>(markerPosition.Item2, markerPosition.Item3) };
        var maxX = grid.GetLength(0) - 1;
        var maxY = grid.GetLength(1) - 1;

        while (true)
        {
            var isObstructed = false;
            var newX = markerPosition.Item2 + directions[markerPosition.Item1, 0];
            var newY = markerPosition.Item3 + directions[markerPosition.Item1, 1];
            var newDirection = markerPosition.Item1;

            if (newX < 0 || newX > maxX || newY < 0 || newY > maxY)
                break;

            if (grid[newX, newY] == obstruction)
            {
                newX = markerPosition.Item2;
                newY = markerPosition.Item3;
                newDirection = markerPosition.Item1 + 1;
                if (newDirection >= markers.Count)
                    newDirection = 0;
            }

            markerPosition = new Tuple<int, int, int>(newDirection, newX, newY);

            if (!isObstructed)
            {
                var notUnique = uniquePositions.Any(x => x.Item1 == newX && x.Item2 == newY);
                if (!notUnique)
                    uniquePositions.Add(new Tuple<int, int>(newX, newY));
            }
        }

        Console.WriteLine($"Amount of unique positions: {uniquePositions.Count}");

        return Task.CompletedTask;
    }
}