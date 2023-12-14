using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    /*
     * Part 1:
     *
     * The engine schematic (your puzzle input) consists of a visual representation of the engine.
     * There are lots of numbers and symbols you don't really understand, but apparently any
     * number adjacent to a symbol, even diagonally, is a "part number" and should be included in
     * your sum. (Periods (.) do not count as a symbol.)
     * 
     * Here is an example engine schematic:
     * 
     * 467..114..
     * ...*......
     * ..35..633.
     * ......#...
     * 617*......
     * .....+.58.
     * ..592.....
     * ......755.
     * ...$.*....
     * .664.598..
     *
     * In this schematic, two numbers are not part numbers because they are not adjacent to a
     * symbol: 114 (top right) and 58 (middle right). Every other number is adjacent to a symbol
     * and so is a part number; their sum is 4361.
     *
     * Of course, the actual engine schematic is much larger. What is the sum of all of the part
     * numbers in the engine schematic?
     *
     *
     * Part 2:
     *
     * The missing part wasn't the only issue - one of the gears in the engine is wrong. A gear
     * is any * symbol that is adjacent to exactly two part numbers. Its gear ratio is the result
     * of multiplying those two numbers together.
     *
     * This time, you need to find the gear ratio of every gear and add them all up so that the
     * engineer can figure out which gear needs to be replaced.
     */
    public class Day03
    {
        [Test]
        public void Part()
        {
            var input = File.ReadAllLines("Input3.txt");

            var result1 = 0;

            var stars = new Dictionary<(int, int), List<int>>();

            for (var i = 0; i < input.Length; i++)
            {
                var line = input[i];

                // Use a regex to find all the numbers in the line
                var matches = Regex.Matches(line, @"(\d+)");
                foreach (Match match in matches)
                {
                    // Calculate the bounds of the surrounding area to search for a symbol
                    var iMin = Math.Max(i - 1, 0);
                    var iMax = Math.Min(i + 1, input.Length - 1);
                    var jMin = Math.Max(match.Index - 1, 0);
                    var jMax = Math.Min(match.Index + match.Length, line.Length - 1);

                    var isPart = false;
                    for (var i2 = iMin; i2 <= iMax && !isPart; i2++)
                    {
                        for (var j2 = jMin; j2 <= jMax && !isPart; j2++)
                        {
                            switch (input[i2][j2])
                            {
                                case '0':
                                case '1':
                                case '2':
                                case '3':
                                case '4':
                                case '5':
                                case '6':
                                case '7':
                                case '8':
                                case '9':
                                case '.':
                                    break;
                                case '*':
                                    isPart = true;
                                    var key = (i2, j2);
                                    if (!stars.TryGetValue(key, out var adjacentPartNumbers))
                                    {
                                        adjacentPartNumbers = new List<int>();
                                        stars.Add(key, adjacentPartNumbers);
                                    }

                                    adjacentPartNumbers.Add(int.Parse(match.Value));
                                    break;
                                default:
                                    // If it's not a digit or a dot, it's a symbol so the number must be a part
                                    isPart = true;
                                    break;
                            }
                        }
                    }

                    if (isPart)
                    {
                        result1 += int.Parse(match.Value);
                    }
                }
            }

            Console.WriteLine($"Part 1: {result1}");

            var gears = stars.Where(kvp => kvp.Value.Count == 2);

            var result2 = gears.Select(kvp => kvp.Value[0] * kvp.Value[1]).Sum();

            Console.WriteLine($"Part 2: {result2}");
        }
    }
}