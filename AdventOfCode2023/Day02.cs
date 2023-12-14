using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public class Day02
    {
        /*
         * For example, the record of a few games might look like this:
         *  
         * Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
         * Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
         * Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
         * Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
         * Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
         *
         * In game 1, three sets of cubes are revealed from the bag (and then put back again). The
         * first set is 3 blue cubes and 4 red cubes; the second set is 1 red cube, 2 green cubes,
         * and 6 blue cubes; the third set is only 2 green cubes.
         * 
         * The Elf would first like to know which games would have been possible if the bag
         * contained only 12 red cubes, 13 green cubes, and 14 blue cubes?
         */
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
                    /*
                     * Determine which games would have been possible if the bag had been loaded
                     * with only 12 red cubes, 13 green cubes, and 14 blue cubes.
                     */
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

                /*
                 * What is the sum of the IDs of those games?
                 */
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