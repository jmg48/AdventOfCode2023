namespace AdventOfCode2023
{
    public class Aoife01 : Aoc
    {
        [Test]
        public void Part1()
        {
            var input = InputLines().ToArray();

            var total = 0;
            foreach (var line in input)
            {
                Console.WriteLine(line);
                char? first = null;
                foreach (var c in line)
                {
                    Console.WriteLine($"The current character is '{c}'");
                    if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
                    {
                        Console.WriteLine("Hooray! it's a digit");
                        first = c;
                        break;
                    }
                }

                char? last = null;
                foreach (var c in line)
                {
                    Console.WriteLine($"The current character is '{c}'");
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
                            Console.WriteLine("Hooray! it's a digit");
                            last = c;
                            break;
                    }
                }

                Console.WriteLine($"{first}{last}");

                var calibrationValue = int.Parse($"{first}{last}");

                total = total + calibrationValue;
            }

            Console.WriteLine($"The total is {total}");
        }
    }
}