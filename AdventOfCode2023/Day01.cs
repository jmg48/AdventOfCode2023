using System.ComponentModel;

namespace AdventOfCode2023
{
    public class Day01
    {
        [Test]
        public void Part1()
        {
            var input = File.ReadAllLines("Input1.txt");

            var result = 0;
            foreach (var line in input)
            {
                var found = false;
                var first = 0;
                var last = 0;

                void add(char c)
                {
                    var i = int.Parse(c.ToString());
                    if (!found)
                    {
                        first = i;
                        found = true;
                    }

                    last = i;
                }

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
                            add(c);
                            break;
                    }
                }

                result += 10 * first + last;
            }

            Console.WriteLine(result);
        }

        [Test]
        public void Part2()
        {
            var input = File.ReadAllLines("Input1.txt");
            
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
                var found = false;
                var first = 0;
                var last = 0;

                void Add(int i)
                {
                    if (!found)
                    {
                        first = i;
                        found = true;
                    }

                    last = i;
                }

                for (var i = 0; i < line.Length; i++)
                {
                    foreach (var (key, value) in map)
                    {
                        if (i + key.Length <= line.Length && line.Substring(i, key.Length) == key)
                        {
                            Add(value);
                        }
                    }
                }

                result += 10 * first + last;
            }

            Console.WriteLine(result);
        }
    }
}