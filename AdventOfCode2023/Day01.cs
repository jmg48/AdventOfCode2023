namespace AdventOfCode2023
{
    public class Day01 : Aoc
    {
        [Test]
        public void Part1()
        {
            var result = 0;
            foreach (var line in InputLines())
            {
                var found = new List<int>();
                foreach (var c in line)
                {
                    switch (c)
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
                            // Accumulate all the digits into a list...
                            found.Add(int.Parse(c.ToString()));
                            break;
                    }
                }

                // ...then add the number formed by the first and last digits to the result
                result += (10 * found[0]) + found[^1];
            }

            Console.WriteLine(result);
        }

        [Test]
        public void Part2()
        {
            var map = new Dictionary<string, int>
            {
                { "0", 0 },
                { "1", 1 },
                { "2", 2 },
                { "3", 3 },
                { "4", 4 },
                { "5", 5 },
                { "6", 6 },
                { "7", 7 },
                { "8", 8 },
                { "9", 9 },
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4 },
                { "five", 5 },
                { "six", 6 },
                { "seven", 7 },
                { "eight", 8 },
                { "nine", 9 },
            };

            var result = 0;
            foreach (var line in InputLines())
            {
                var found = new List<int>();
                for (var i = 0; i < line.Length; i++)
                {
                    foreach (var (key, value) in map)
                    {
                        // Process is identical to part 1, just matching a longer substring without overrunning the end of the string
                        if (i + key.Length <= line.Length && line.Substring(i, key.Length) == key)
                        {
                            found.Add(value);
                        }
                    }
                }

                result += (10 * found[0]) + found[^1];
            }

            Console.WriteLine(result);
        }
    }
}