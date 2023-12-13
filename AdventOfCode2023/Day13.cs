using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.VisualBasic;

namespace AdventOfCode2023
{
    public class Day13
    {
        [TestCase(1)]
        [TestCase(2)]
        public void Part(int part)
        {
            var target = part switch { 1 => 0, 2 => 1 };

            var input = File.ReadLines("Input13.txt").ToList();
            var patterns = new List<List<string>> { new() };
            foreach (var line in input)
            {
                if (line.Length == 0)
                {
                    patterns.Add(new List<string>());
                }
                else
                {
                    patterns[^1].Add(line); 
                }
            }

            var result = 0;
            foreach (var pattern in patterns)
            {
                for (var i = 1; i < pattern.Count; i++)
                {
                    var smudges = 0;
                    var j = i;
                    var k = i - 1;
                    while (j < pattern.Count && k >= 0)
                    {
                        smudges += pattern[j].Zip(pattern[k]).Count(it => it.First != it.Second);

                        j++;
                        k--;
                    }

                    if (smudges == target)
                    {
                        result += 100 * i;
                    }
                }

                for (var i = 1; i < pattern[0].Length; i++)
                {
                    var smudges = 0;
                    var j = i;
                    var k = i - 1;
                    while (j < pattern[0].Length && k >= 0)
                    {
                        smudges += pattern.Count(row => row[j] != row[k]);
                        j++;
                        k--;
                    }

                    if (smudges == target)
                    {
                        result += i;
                    }
                }
            }

            Console.WriteLine(result);
        }
    }
}