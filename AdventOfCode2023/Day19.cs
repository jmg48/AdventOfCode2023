using System.Data;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public class Day19
    {
        [Test]
        public void Part()
        {
            var input = File.ReadAllText("Input19.txt").Split("\n\n");

            var workflows = input[0].Split("\n").Select(it =>
            {
                var match = Regex.Match(it, @"(\w+){(.*)}");
                return (match.Groups[1].Value, match.Groups[2].Value.Split(","));
            }).ToDictionary(it => it.Item1, it => it.Item2);

            long result = 0;
            var parts = new Stack<(string, Dictionary<string, (int Low, int High)>)>();
            parts.Push(("in",new Dictionary<string, (int, int)>
            {
                {"x", (1, 4000)},
                {"m", (1, 4000)},
                {"a", (1, 4000)},
                {"s", (1, 4000)},
            }));

            while (parts.TryPop(out var it))
            {
                var (ruleKey, part) = it;

                if (part.Values.Any(it => it.High < it.Low))
                {
                    // If the part range has no legal values for one of the categories then we're done with it
                    continue;
                }

                switch (ruleKey)
                {
                    // "A" is a special rule key meaning we Accept the part range and add its combinations to the result
                    case "A":
                        result += part.Values
                            .Select(it => Math.Max(0, it.High - it.Low + 1))
                            .Aggregate((long)1, (a, b) => a * b);
                        continue;
                    // "R" is a special rule key meaning we Reject the part range
                    case "R":
                        continue;
                }

                // Otherwise we process the rules in the workflow associated with the rule key
                var workflow = workflows[ruleKey];
                foreach (var rule in workflow)
                {
                    if (!rule.Contains(":"))
                    {
                        // If the rule doesn't have a condition then we just "goto" the workflow specified by the rule
                        parts.Push((rule, part));
                        break;
                    }

                    var split = Regex.Match(rule, @"(\w+)(<|>)(\d+):(\w+)");
                    var op = split.Groups[2].Value;
                    var r = int.Parse(split.Groups[3].Value);
                    var d = split.Groups[4].Value;

                    // We split the incoming part range into a 
                    var lo = part.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                    var hi = part.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                    lo[split.Groups[1].Value] = (lo[split.Groups[1].Value].Low, op switch { "<" => r - 1, ">" => r });
                    hi[split.Groups[1].Value] = (op switch { "<" => r, ">" => r + 1 }, hi[split.Groups[1].Value].High);
                    
                    parts.Push((d, op switch { "<" => lo, ">" => hi }));
                    part = op switch { "<" => hi, ">" => lo };

                    Console.WriteLine(string.Join(",", lo.Select(kvp => $"{kvp.Key}={kvp.Value}")));
                    Console.WriteLine(string.Join(",", hi.Select(kvp => $"{kvp.Key}={kvp.Value}")));

                }
            }

            Console.WriteLine(result);
        }
    }
}