using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public class Day03 : Aoc
    {
        [Test]
        public void Part()
        {
            var input = InputLines().ToArray();

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