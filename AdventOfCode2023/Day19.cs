using System.Data;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public class Day19
    {
        [Test]
        public void Part1()
        {
            var input = File.ReadAllText("Input19.txt").Split("\n\n");

            var workflows = input[0].Split("\n").Select(it =>
            {
                var match = Regex.Match(it, @"(\w+){(.*)}");
                return (match.Groups[1].Value, match.Groups[2].Value.Split(","));
            }).ToDictionary(it => it.Item1, it => it.Item2);

            var parts = input[1]
                .Split("\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(it => it[1..^1]).Select(it =>
                {
                    var split = it.Split('=', ',');
                    return new Dictionary<string, int>
                    {
                        {split[0], int.Parse(split[1]) },
                        {split[2], int.Parse(split[3]) },
                        {split[4], int.Parse(split[5]) },
                        {split[6], int.Parse(split[7]) },
                    };
                })
                .ToList();

            var result = 0;
            foreach (var part in parts)
            {
                var workflowKey = "in";
                while (true)
                {
                    if (workflowKey == "A")
                    {
                        // "A" is a special workflow key meaning we Accept the part and add its values to the result
                        result += part.Values.Sum();
                        break;
                    }

                    if (workflowKey == "R")
                    {
                        // "R" is a special workflow key meaning we Reject the part
                        break;
                    }

                    foreach (var rule in workflows[workflowKey])
                    {
                        if (!rule.Contains(":"))
                        {
                            // If the rule doesn't have a condition then we jump to the workflow specified by the rule
                            workflowKey = rule;
                            break;
                        }

                        var match = Regex.Match(rule, @"(\w+)(<|>)(\d+):(\w+)").Groups;
                        var (xmas, op, val, dest) = (match[1].Value, match[2].Value, int.Parse(match[3].Value), match[4].Value);

                        if (op switch { "<" => part[xmas] < val, ">" => part[xmas] > val })
                        {
                            // If the part matches the rule condition then we jump to the workflow specified by the rule
                            workflowKey = dest;
                            break;
                        }
                    }
                }
            }

            Console.WriteLine(result);
        }

        [Test]
        public void Part2()
        {
            var input = File.ReadAllText("Input19.txt").Split("\n\n");

            var workflows = input[0].Split("\n").Select(it =>
            {
                var match = Regex.Match(it, @"(\w+){(.*)}");
                return (match.Groups[1].Value, match.Groups[2].Value.Split(","));
            }).ToDictionary(it => it.Item1, it => it.Item2);

            long result = 0;
            var parts = new Stack<(string, Dictionary<string, (int Low, int High)>)>();
            parts.Push(("in", new Dictionary<string, (int, int)>
            {
                { "x", (1, 4000) },
                { "m", (1, 4000) },
                { "a", (1, 4000) },
                { "s", (1, 4000) },
            }));

            while (parts.TryPop(out var it))
            {
                var (ruleKey, range) = it;

                if (range.Values.Any(it => it.High < it.Low))
                {
                    // If the part range has no legal values for one of the categories then we're done with it
                    continue;
                }

                switch (ruleKey)
                {
                    // "A" is a special workflow key meaning we Accept the part range and add its combinations to the result
                    case "A":
                        result += range.Values.Select(it => Math.Max(0, it.High - it.Low + 1)).Aggregate((long)1, (a, b) => a * b);
                        continue;
                    // "R" is a special workflow key meaning we Reject the part range
                    case "R":
                        continue;
                }

                // Otherwise we process the rules in the workflow associated with the rule key
                var workflow = workflows[ruleKey];
                foreach (var rule in workflow)
                {
                    if (!rule.Contains(":"))
                    {
                        // If the rule doesn't have a condition then we jump to the workflow specified by the rule
                        parts.Push((rule, range));
                        break;
                    }

                    var match = Regex.Match(rule, @"(\w+)(<|>)(\d+):(\w+)").Groups;
                    var (xmas, op, val, dest) = (match[1].Value, match[2].Value, int.Parse(match[3].Value), match[4].Value);

                    // We split the incoming part range into "low" and "high" sub-ranges based on the rule condition
                    var lo = range.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                    var hi = range.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                    lo[xmas] = (lo[xmas].Low, op switch { "<" => val - 1, ">" => val });
                    hi[xmas] = (op switch { "<" => val, ">" => val + 1 }, hi[xmas].High);
                    
                    // For the sub-range that matches the condition, push it onto the stack for processing against the destination workflow
                    parts.Push((dest, op switch { "<" => lo, ">" => hi }));

                    // For the sub-range that doesn't match the condition, continue processing the remaining rules in the workflow
                    range = op switch { "<" => hi, ">" => lo };
                }
            }

            Console.WriteLine(result);
        }
    }
}