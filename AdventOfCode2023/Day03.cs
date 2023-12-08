using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public class Day03
    {
        [Test]
        public void Part1()
        {
            var input = File.ReadAllLines("Input3.txt");

            //input = new[] {
            //    "467..114..",
            //    "...*......",
            //    "..35..633.",
            //    "......#...",
            //    "617*......",
            //    ".....+.58.",
            //    "..592.....",
            //    "......755.",
            //    "...$.*....",
            //    ".664.598.." };

            var result = 0;

            for (var i = 0; i < input.Length; i++)
            {
                var line = input[i];
                var matches = Regex.Matches(line, @"(\d+)");
                foreach(Match match in matches)
                {
                    //Console.WriteLine(match.Value + " at " + match.Index);

                    var isPart = false;
                    var iMin = Math.Max(i - 1, 0);
                    var iMax = Math.Min(i + 1, input.Length - 1);
                    var jMin = Math.Max(match.Index - 1, 0);
                    var jMax = Math.Min(match.Index + match.Length, line.Length - 1);
                    for (var i2 = iMin; i2 <= iMax && !isPart; i2++) 
                    { 
                        for(var j2 = jMin; j2 <= jMax && !isPart; j2++)
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
                                default:
                                    isPart = true;
                                    break;
                            }
                        }
                    }

                    if(isPart)
                    {
                        result += int.Parse(match.Value);
                    }
                }
            }

            Console.WriteLine(result);
        }

        [Test]
        public void Part2()
        {
            var input = File.ReadAllLines("Input3.txt");

            //input = new[] {
            //    "467..114..",
            //    "...*......",
            //    "..35..633.",
            //    "......#...",
            //    "617*......",
            //    ".....+.58.",
            //    "..592.....",
            //    "......755.",
            //    "...$.*....",
            //    ".664.598.." };

            var result = new Dictionary<(int, int), List<int>>();

            for (var i = 0; i < input.Length; i++)
            {
                var line = input[i];
                var matches = Regex.Matches(line, @"(\d+)");
                foreach (Match match in matches)
                {
                    //Console.WriteLine(match.Value + " at " + match.Index);

                    var isPart = false;
                    var iMin = Math.Max(i - 1, 0);
                    var iMax = Math.Min(i + 1, input.Length - 1);
                    var jMin = Math.Max(match.Index - 1, 0);
                    var jMax = Math.Min(match.Index + match.Length, line.Length - 1);
                    for (var i2 = iMin; i2 <= iMax && !isPart; i2++)
                    {
                        for (var j2 = jMin; j2 <= jMax && !isPart; j2++)
                        {
                            switch (input[i2][j2])
                            {
                                case '*':
                                    isPart = true;
                                    var key = (i2, j2);
                                    if(!result.TryGetValue(key, out var value))
                                    {
                                        value = new List<int>();
                                        result.Add(key, value);
                                    }

                                    value.Add(int.Parse(match.Value));
                                    break;
                            }
                        }
                    }
                }
            }

            Console.WriteLine(result.Where(kvp => kvp.Value.Count == 2).Select(kvp => kvp.Value[0] * kvp.Value[1]).Sum());
        }
    }
}