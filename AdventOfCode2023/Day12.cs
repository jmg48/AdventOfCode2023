using System.Diagnostics;

namespace AdventOfCode2023
{
    public class Day12 : Aoc
    {
        [TestCase(1)]
        [TestCase(2)]
        public void Part(int part)
        {
            var timer = new Stopwatch();
            timer.Start();

            var input = InputLines().ToList();

            var tasks = input.Select(line => Task.Factory.StartNew(() =>
            {
                var pattern = line.Split(" ")[0];
                var known = line.Split(" ")[1].Split(",").Select(int.Parse).ToList();

                if (part == 2)
                {
                    var repeats = 5;
                    pattern = string.Join("?", Enumerable.Repeat(pattern, repeats));
                    known = Enumerable.Repeat(known, repeats).SelectMany(i => i).ToList();
                }

                var timer = new Stopwatch();
                timer.Start();
                var arrangements = Arrangements(pattern, known);

                return (arrangements, timer.ElapsedMilliseconds);
            })).ToList();

            long result = 0;
            while (tasks.Count > 0)
            {
                var waitAny = Task.WaitAny(tasks.ToArray<Task>(), TimeSpan.FromSeconds(10));
                var printRemaining = true;
                if (waitAny >= 0)
                {
                    var (arrangements, elapsedMilliseconds) = tasks[waitAny].Result;
                    result += arrangements;
                    tasks.RemoveAt(waitAny);
                    input.RemoveAt(waitAny);

                    //File.AppendAllLines(outFile, new[] { $"Found {arrangements} arrangements in {elapsedMilliseconds}ms - interim result is {result} with {input.Count} tasks remaining" });
                    printRemaining = true;
                }
                else if (printRemaining)
                {
                    //File.AppendAllLines(outFile, new[] { "Remaining tasks:" }.Concat(input));
                    printRemaining = false;
                }
            }

            Console.WriteLine($"{result} in {timer.ElapsedMilliseconds}ms");
        }

        private long Arrangements(string pattern, IReadOnlyList<int> sections) =>
            Arrangements(pattern, sections, 0, pattern.Length);

        private long Arrangements(string pattern, IReadOnlyList<int> sections, int patternOffset, int patternLength) =>
            sections.Count switch
            {
                0 => NoSections(pattern, patternOffset, patternLength),
                1 => OneSection(pattern, sections[0], patternOffset, patternLength),
                _ => MultipleSections(pattern, sections, patternOffset, patternLength)
            };

        private long MultipleSections(string pattern, IReadOnlyList<int> sections, int patternOffset, int patternLength)
        {
            // If there's more than one section, split the sections into three smaller sub-problems
            var leftSections = sections.Take(sections.Count / 2).ToList();
            var pivotSection = sections[sections.Count / 2];
            var rightSections = sections.Skip(sections.Count / 2 + 1).ToList();

            // For each possible position of the pivot section...
            var beforePivotMin = leftSections.Sum() + leftSections.Count - 1;
            var beforePivot = beforePivotMin;
            var slack = patternLength - sections.Sum() - sections.Count + 2;
            long result = 0;
            for (var i = 0; i < slack; i++, beforePivot++)
            {
                // Check points before and after the pivot section are empty - if not then we're done
                var afterPivot = beforePivot + 1 + pivotSection;
                if (pattern[patternOffset + beforePivot] == '#' || (afterPivot < patternLength && pattern[patternOffset + afterPivot] == '#'))
                {
                    continue;
                }

                // Check the pivot section is legally placed - if not then we're done
                var middle = OneSection(pattern, pivotSection, patternOffset + beforePivot + 1, pivotSection);
                if (middle == 0)
                {
                    continue;
                }

                // Check how many ways the left section(s) can be placed in the space to the left of the pivot - if none then we're done
                var left = Arrangements(pattern, leftSections, patternOffset, beforePivot);
                if (left == 0)
                {
                    continue;
                }

                // Check how many ways the right section(s) can be placed in the space to the right of the pivot
                var right = Arrangements(pattern, rightSections, patternOffset + afterPivot + 1, patternLength - afterPivot - 1);

                // Add to the result the combined permutations of left and right
                result += left * right;
            }

            return result;
        }

        private long OneSection(string pattern, int section, int patternOffset, int patternLength)
        {
            // If there's only one section, then try it in each of its possible positions
            var slack = patternLength - section - 1 + 2;
            long result = 0;
            for (var i = 0; i < slack; i++)
            {
                var x = 0;
                var isPossible = true;
                for (; isPossible && x < i; x++)
                {
                    if (pattern[patternOffset + x] == '#')
                    {
                        isPossible = false;
                    }
                }

                for (; isPossible && x < i + section; x++)
                {
                    if (pattern[patternOffset + x] == '.')
                    {
                        isPossible = false;
                    }
                }

                for (; isPossible && x < patternLength; x++)
                {
                    if (pattern[patternOffset + x] == '#')
                    {
                        isPossible = false;
                    }
                }

                if (isPossible)
                {
                    // If the position is legal then it counts as one possible arrangement
                    result++;
                }
            }

            return result;
        }

        private static long NoSections(string pattern, int patternOffset, int patternLength)
        {
            // If there are no sections, check the space can be entirely empty
            for (var x = 0; x < patternLength; x++)
            {
                if (pattern[patternOffset + x] == '#')
                {
                    return 0;
                }
            }

            return 1;
        }
    }
}