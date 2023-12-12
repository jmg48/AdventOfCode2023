using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.VisualBasic;

namespace AdventOfCode2023
{
    public class Day12
    {
        [TestCase(1)]
        [TestCase(2)]
        public void Part(int part)
        {
            var timer = new Stopwatch();
            timer.Start();

            //var outFile = "C:\\src\\Day12.txt";
            //File.WriteAllLines(outFile, new[] { "Day 12 Part 2"});

            var input = File.ReadLines("Input12.txt").ToList();

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

        private long Arrangements(string pattern, List<int> known)
        {
            return Arrangements(pattern, known, 0, pattern.Length, "Begin");
        }

        private long Arrangements(string pattern, List<int> known, int patternOffset, int patternLength, string comment)
        {
            //Console.WriteLine($"{comment}: {new string(' ', patternOffset)}{pattern.Substring(patternOffset, patternLength)} {string.Join(",", known)}");
            long result = 0;

            var pivots = patternLength - known.Sum() - known.Count + 2;

            if (known.Count > 1)
            {
                var knownLeft = known.Take(known.Count / 2).ToList();
                var knownMiddle = known[known.Count / 2];
                var knownRight = known.Skip(known.Count / 2 + 1).ToList();

                var minPivot = knownLeft.Sum() + knownLeft.Count - 1;

                var pivot = minPivot;
                for (var i = 0; i < pivots; i++, pivot++)
                {
                    var pivot2 = pivot + 1 + knownMiddle;
                    //Console.WriteLine($"Pivot: {new string(' ', patternOffset + pivot)}.{new string('#', knownMiddle)}. = {pivot}");

                    // Pivot point cannot be filled
                    if (pattern[patternOffset + pivot] == '#' || (pivot2 < patternLength && pattern[patternOffset + pivot2] == '#'))
                    {
                        continue;
                    }

                    var middle = Arrangements(pattern, new List<int> { knownMiddle }, patternOffset + pivot + 1, knownMiddle, "Mid  ");
                    if (middle == 0)
                    {
                        continue;
                    }

                    var left = Arrangements(pattern, knownLeft, patternOffset, pivot, "Left ");
                    if (left == 0)
                    {
                        continue;
                    }

                    if (knownRight.Count > 0)
                    {
                        var right = Arrangements(pattern, knownRight,  patternOffset + pivot2 + 1, patternLength - pivot2 - 1, "Right");
                        result += left * right;
                    }
                    else if (pivot2 < patternLength)
                    {
                        var isPossible = true;
                        for (var x = pivot2; x < patternLength; x++)
                        {
                            if (pattern[patternOffset + x] == '#')
                            {
                                isPossible = false;
                            }
                        }

                        if (!isPossible)
                        {
                            continue;
                        }

                        result += left;
                    }
                    else
                    {
                        result += left;
                    }
                }
            }
            else
            {
                for (var i = 0; i < pivots; i++)
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

                    for (; isPossible && x < i + known[0]; x++)
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
                        result++;
                    }
                }
            }

            //Console.WriteLine($"{comment}: {new string(' ', patternOffset)}{pattern.Substring(patternOffset, patternLength)} {string.Join(",", known)} = {result}");
            return result;
        }
    }
}