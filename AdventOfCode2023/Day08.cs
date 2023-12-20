using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public class Day08
    {
        [Test]
        public void Part1()
        {
            var input = File.ReadAllLines("Input8.txt");

            var directions = input[0];

            var nodes = input.Skip(2).Select(line =>
            {
                var match = Regex.Match(line, @"(\w\w\w) = \((\w\w\w), (\w\w\w)\)");
                return (key: match.Groups[1].Value, left: match.Groups[2].Value, right: match.Groups[3].Value);
            }).ToDictionary(x => x.key, x => (x.key, x.left, x.right));

            var currentNode = nodes["AAA"];

            var steps = 0;
            while (true)
            {
                foreach (var direction in directions)
                {
                    currentNode = nodes[direction == 'L' ? currentNode.left : currentNode.right];
                    steps++;
                    if (currentNode.key == "ZZZ")
                    {
                        Console.WriteLine(steps);
                        return;
                    }
                }
            }
        }

        [Test]
        public void Part2()
        {
            var input = File.ReadAllLines("Input8.txt");

            var directions = input[0];

            var nodes = input.Skip(2).Select(line =>
            {
                var match = Regex.Match(line, @"(\w\w\w) = \((\w\w\w), (\w\w\w)\)");
                return (key: match.Groups[1].Value, left: match.Groups[2].Value, right: match.Groups[3].Value);
            }).ToDictionary(x => x.key, x => (x.key, x.left, x.right));

            var currentNodes = nodes.Values.Where(node => node.key[2] == 'A').ToList();

            var arrivals = new Dictionary<int, List<int>>();

            var steps = 0;
            while (arrivals.Values.All(x => x.Count < 50))
            {
                foreach (var direction in directions)
                {
                    currentNodes = currentNodes.Select(currentNode =>  nodes[direction == 'L' ? currentNode.left : currentNode.right]).ToList();
                    steps++;

                    for (var j = 0; j < currentNodes.Count; j++)
                    {
                        if (currentNodes[j].key[2] != 'Z')
                        {
                            continue;
                        }

                        // Even if we haven't arrived in every case, record each arrival to see if there's a pattern
                        if (!arrivals.TryGetValue(j, out var values))
                        {
                            values = new List<int>();
                            arrivals.Add(j, values);
                        }

                        values.Add(steps);
                    }
                }
            }

            // Can't prove why this should be the case, but by observation every route has a constant period over which it returns to a finish point
            var cycleLengths = arrivals.Values.Select(x =>
            {
                var gaps = x.Skip(1).Zip(x, (a, b) => a - b).ToList();
                return gaps.Distinct().Single();
            }).OrderByDescending(x => x).ToList();

            Console.WriteLine(string.Join(", ", cycleLengths));

            foreach (var cycleLength in cycleLengths)
            {
                Console.WriteLine(string.Join(", ", PrimeFactorsOf(cycleLength)));
            }
            
            // Luckily all the prime factors are unique so simple product of distinct values give the correct solution
            Console.WriteLine(cycleLengths.SelectMany(PrimeFactorsOf).Distinct().Aggregate((long)1, (a, b) => a * b));
        }

        // Quick-and-dirty prime sieve
        public static IEnumerable<int> PrimeFactorsOf(int value)
        {
            var primes = new HashSet<int>();
            for (var i = 2; i * i <= value; i++)
            {
                var isPrime = true;
                foreach (var prime in primes)
                {
                    if (i % prime == 0)
                    {
                        isPrime = false;
                    }
                }

                if (isPrime)
                {
                    primes.Add(i);
                    while (value % i == 0)
                    {
                        yield return i;
                        value /= i;
                    }
                }
            }

            if (value != 1)
            {
                yield return value;
            }
        }
    }
}