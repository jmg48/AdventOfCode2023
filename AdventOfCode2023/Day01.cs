namespace AdventOfCode2023
{
    public class Day01
    {
        /*
         * The newly-improved calibration document consists of lines of text; each line originally
         * contained a specific calibration value that the Elves now need to recover. On each line,
         * the calibration value can be found by combining the first digit and the last digit (in
         * that order) to form a single two-digit number.
         *
         * For example:
         *
         * 1abc2
         * pqr3stu8vwx
         * a1b2c3d4e5f
         * treb7uchet
         *
         * In this example, the calibration values of these four lines are 12, 38, 15, and 77.
         * Adding these together produces 142.
         *
         * Consider your entire calibration document. What is the sum of all of the calibration
         * values?
         */

        [Test]
        public void Part1()
        {
            var input = File.ReadAllLines("Input1.txt");

            /*
             * On each line, the calibration value can be found by combining the first digit and
             * the last digit (in that order) to form a single two-digit number.
             */
            var result = 0;
            foreach (var line in input)
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
            var input = File.ReadAllLines("Input1.txt");

            /*
             * It looks like some of the digits are actually spelled out with letters: one, two,
             * three, four, five, six, seven, eight, and nine also count as valid "digits".
             */
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
            foreach (var line in input)
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