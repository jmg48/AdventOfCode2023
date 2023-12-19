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

            var rules = input[0].Split("\n").Select(it =>
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
                var done = false;
                var rule = rules["in"];
                while (!done)
                {
                    foreach (var rulePart in rule)
                    {
                        if (rulePart.Contains(":"))
                        {
                            var split = Regex.Match(rulePart, @"(\w+)(<|>)(\d+):(\w+)");
                            var l = part[split.Groups[1].Value];
                            var op = split.Groups[2].Value;
                            var r = int.Parse(split.Groups[3].Value);
                            var d = split.Groups[4].Value;
                            var test = op switch { "<" => l < r, ">" => l > r };
                            if (test)
                            {
                                if (d == "A")
                                {
                                    result += part.Values.Sum();
                                    done = true;
                                    break;
                                }
                                else if (d == "R")
                                {
                                    done = true;
                                    break;
                                }
                                else
                                {
                                    rule = rules[d];
                                    break;
                                }
                            }
                        }
                        else if (rulePart == "A")
                        {
                            result += part.Values.Sum();
                            done = true;
                            break;
                        }
                        else if (rulePart == "R")
                        {
                            done = true;
                            break;
                        }
                        else
                        {
                            rule = rules[rulePart];
                            break;
                        }
                    }
                }
            }

            Console.WriteLine(result);
        }
    }
}