using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public class Day20 : Aoc
    {
        [TestCase(1)]
        [TestCase(2)]
        public void Part(int part)
        {
            var input = InputLines();
            var modules = new Dictionary<string, (string, List<string>, Dictionary<string, bool>)>();
            foreach (var line in input)
            {
                var split = line.Split(" -> ");
                var left = Regex.Match( split[0], @"(%|&)?(\w+)");
                modules.Add(left.Groups[2].Value, (left.Groups[1].Value, split[1].Split(", ").ToList(), new Dictionary<string, bool>()));
            }

            foreach (var module in modules)
            {
                foreach (var dest in module.Value.Item2)
                {
                    if (modules.TryGetValue(dest, out var destModule))
                    {
                        if (destModule.Item1 == "&")
                        {
                            destModule.Item3.Add(module.Key, false);
                        }
                    }
                }

                if (module.Value.Item1 == "%")
                {
                    module.Value.Item3.Add("", false);
                }
            }

            var hi = 0;
            var lo = 0;
            var cycles = new Dictionary<string, List<int>>();
            for (var i = 0; i < part switch { 1 => 1000, 2 => 100000 }; i++)
            {
                var work = new Queue<(string, string, bool)>();
                lo++;
                foreach (var dest in modules["broadcaster"].Item2)
                {
                    work.Enqueue(("broadcaster", dest, false));
                }

                while (work.TryDequeue(out var x))
                {
                    var (from, to, pulse) = x;

                    if (pulse)
                    {
                        hi++;
                    }
                    else
                    {
                        lo++;
                    }

                    if (part == 2)
                    {
                        if (to == "cs" && pulse)
                        {
                            if (!cycles.TryGetValue(from, out var values))
                            {
                                values = new List<int>();
                                cycles[from] = values;
                            }

                            values.Add(i + 1);
                        }
                    }

                    if (modules.TryGetValue(to, out var module))
                    {
                        switch (module.Item1)
                        {
                            case "%":
                                if (!pulse)
                                {
                                    module.Item3[""] = !module.Item3[""];

                                    foreach (var dest in module.Item2)
                                    {
                                        work.Enqueue((to, dest, module.Item3[""]));
                                    }
                                }

                                break;
                            case "&":
                                var memory = module.Item3;
                                memory[from] = pulse;
                                var nextState = !memory.Values.All(x => x);

                                foreach (var dest in module.Item2)
                                {
                                    work.Enqueue((to, dest, nextState));
                                }

                                break;
                        }
                    }
                }
            }

            switch (part)
            {
                case 1:
                    Console.WriteLine((long)lo * hi);
                    break;
                case 2:
                    var primeFactors = new HashSet<int>();
                    foreach (var kvp in cycles)
                    {
                        var cycleLength = kvp.Value.Skip(1).Zip(kvp.Value, (a, b) => a - b).Distinct().Single();
                        Console.Write($"For key {kvp.Key}, cycle length is {cycleLength} with prime factors: ");
                        foreach (var primeFactor in Day08.PrimeFactorsOf(cycleLength))
                        {
                            Console.Write($"{primeFactor}, ");
                            primeFactors.Add(primeFactor);
                        }

                        Console.WriteLine();
                    }

                    Console.WriteLine(primeFactors.Aggregate((long)1, (a, b) => a * b));
                    break;
            }
        }
    }
}