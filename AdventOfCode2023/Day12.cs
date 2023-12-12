using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.VisualBasic;

namespace AdventOfCode2023
{
    public class Day12
    {
        [Test]
        public void Part()
        {
            //var test = Arrangements("???.###", new List<int> { 1, 1, 3 });
            //Console.WriteLine(Arrangements("?###????????", new List<int> { 3, 2, 1 }));
            //return;

            var input = File.ReadLines("Input12.txt").ToList();

            var result = 0;
            foreach (var line in input)
            {
                Console.WriteLine(line);

                var pattern = line.Split(" ")[0];
                var known = line.Split(" ")[1].Split(",").Select(int.Parse).ToList();

                var arrangements = Arrangements(pattern, known);
                Console.WriteLine(arrangements);
                result += arrangements;
            }

            Console.WriteLine(result);
        }

        private int Arrangements(string pattern, List<int> known, int offset = 0, bool verbose = false)
        {
            if (verbose && offset == 0)
            {
                Console.WriteLine(pattern);
            }

            var result = 0;

            var positions = pattern.Length - offset - known.Sum() - known.Count + 2;

            for (var i = 0; i < positions; i++)
            {
                var x = offset;
                var isPossible = true;
                for (var j = 0; isPossible && j < i; j++)
                {
                    if (pattern[x] == '#')
                    {
                        isPossible = false;
                    }

                    x++;
                }

                for (var j = 0; isPossible && j < known[0]; j++)
                {
                    if (pattern[x] == '.')
                    {
                        isPossible = false;
                    }

                    x++;
                }

                if (x < pattern.Length && pattern[x] == '#')
                {
                    isPossible = false;
                }

                x++;

                if (isPossible)
                {
                    if (verbose)
                    {
                        Console.WriteLine(new string(' ', offset + i) + new string('#', known[0]) + '.');
                    }

                    if (known.Count == 1)
                    {
                        for (int j = x; j < pattern.Length; j++)
                        {
                            if (pattern[j] == '#')
                            {
                                isPossible = false;
                            }
                        }

                        if (isPossible)
                        {
                            result++;
                        }
                    }
                    else
                    {
                        result += Arrangements(pattern, known.Skip(1).ToList(), x);
                    }
                }
            }

            return result;
        }

        private bool IsPossible(string pattern, List<int> known)
        {
            var i = 0;
            foreach (var group in known)
            {
                for (int j = 0; j < group; j++)
                {
                    if (pattern[i] == '.')
                    {
                        return false;
                    }

                    i++;
                }

                if (pattern[i] == '#')
                {
                    return false;
                }

                i++;
            }

            return true;
        }
    }
}