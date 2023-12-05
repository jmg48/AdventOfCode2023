using System.Collections.Concurrent;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public class Day04
    {
        [Test]
        public void Part1()
        {
            var input = File.ReadAllLines("Input4.txt");

            var result = 0;
            foreach (var line in input)
            {
                var split = line.Split(':', '|');
                var a = new HashSet<int>(ParseInts(split[1]));
                var b = ParseInts(split[2]);
                var score = 0;
                foreach (var c in b)
                {
                    if (a.Contains(c))
                    {
                        score = score == 0 ? 1 : (score * 2);
                    }
                }

                result += score;
            }

            Console.WriteLine(result);
        }

        [Test]
        public void Part2()
        {
            var input = File.ReadAllLines("Input4.txt");

            var copies = input.Select(s => 1).ToList();

            for (var i = 0; i < input.Length; i++)
            {
                var line = input[i];
                var split = line.Split(':', '|');
                var a = new HashSet<int>(ParseInts(split[1]));
                var b = ParseInts(split[2]);
                var score = 0;
                foreach (var c in b)
                {
                    if (a.Contains(c))
                    {
                        score++;
                    }
                }

                for (int j = 0; j < score; j++)
                {
                    if (i + j + 1 < copies.Count)
                    {
                        copies[i + j + 1] += copies[i];
                    }
                }
            }

            var result = copies.Sum();
            Console.WriteLine(result);
        }

        private List<int> ParseInts(string s)
        {
            return s.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList();
        }
    }
}