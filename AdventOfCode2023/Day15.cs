using System.Text;

namespace AdventOfCode2023
{
    public class Day15
    {
        [Test]
        public void Part1()
        {
            var input = File.ReadLines("Input15.txt").ToList();

            var steps = input[0].Split(',');

            var result = 0;
            foreach (var step in steps)
            {
                var bytes = Encoding.UTF8.GetBytes(step);

                var hash = 0;
                foreach (var code in bytes)
                {
                    hash += code;
                    hash *= 17;
                    hash = hash % 256;
                }

                result += hash;
            }

            Console.WriteLine(result);
        }
    }
}