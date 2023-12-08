using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public class Day02
    {
        [Test]
        public void Part1()
        {
            var input = File.ReadAllLines("Input2.txt");
            var result = 0;

            for (var i = 0; i < input.Length; i++)
            {
                var line = input[i];

                var id = i + 1;
                var possible = true;
                foreach (var colour in line.Split(':')[1].Split(';', ','))
                {
                    var match = Regex.Match(colour, @"(\d+) (red|green|blue)");

                    var limit = match.Groups[2].Value switch
                    {
                        "red" => 12,
                        "green" => 13,
                        "blue" => 14,
                    };

                    if (int.Parse(match.Groups[1].Value) > limit)
                    {
                        possible = false;
                    }
                }

                if (possible)
                {
                    result += id;
                }
            }

            Console.WriteLine(result);
        }

        [Test]
        public void Part2()
        {
            var result = File.ReadLines("Input2.txt")
                .Select(line => line.Split(':')[1].Split(';', ',')
                    .Select(s => Regex.Match(s, @"(\d+) (red|green|blue)"))
                    .GroupBy(match => match.Groups[2].Value, match => int.Parse(match.Groups[1].Value),
                        (_, x) => x.Max())
                    .Aggregate((a, b) => a * b)).Sum();

            Console.WriteLine(result);
        }
    }
}