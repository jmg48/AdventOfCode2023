using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public class Day09
    {
        [Test]
        public void Part()
        {
            var input = File.ReadAllLines("Input9.txt");
            var sequences = input.Select(line => line.Split(" ").Select(long.Parse).ToList());

            long result = 0;
            long result2 = 0;

            foreach (var sequence in sequences)
            {
                var diffs = new Stack<List<long>>();
                diffs.Push(sequence);
                while(diffs.Peek().Any(i => i != 0))
                {
                    diffs.Push(diffs.Peek().Skip(1).Zip(diffs.Peek(), (a, b) => a - b).ToList());
                }

                long lastValue = 0;
                long firstValue = 0;
                diffs.Pop();
                while (diffs.Count > 0)
                {
                    var popped = diffs.Pop();
                    lastValue += popped[^1];
                    firstValue = popped[0] - firstValue;
                }

                result += lastValue;
                result2 += firstValue;
            }

            Console.WriteLine(result);
            Console.WriteLine(result2);
        }
    }
}