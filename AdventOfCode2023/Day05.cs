using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public class Day05 : Aoc
    {
        [TestCase(1)]
        [TestCase(2)]
        public void Part(int part)
        {
            var input = InputLines().ToArray();

            var seeds = input[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();

            var stages = new List<List<Map>>();
            {
                var maps = new List<Map>();
                for (var i = 3; i < input.Length; i++)
                {
                    var line = input[i];
                    var match = Regex.Match(line, @"(\d+) (\d+) (\d+)");
                    if (match.Success)
                    {
                        maps.Add(new Map(long.Parse(match.Groups[1].Value), long.Parse(match.Groups[2].Value), long.Parse(match.Groups[3].Value)));
                    }
                    else if (maps.Count > 0)
                    {
                        stages.Add(maps);
                        maps = new List<Map>();
                    }
                }

                if (maps.Count > 0)
                {
                    stages.Add(maps);
                }
            }

            var result = long.MaxValue;
            var source = new List<Range>();
            for (var i = 0; i < seeds.Count; i += 2)
            {
                if (part == 1)
                {
                    source.Add(new Range(seeds[i], 1));
                    source.Add(new Range(seeds[i + 1], 1));
                }
                else
                {
                    source.Add(new Range(seeds[i], seeds[i + 1]));
                }
            }

            Console.WriteLine($"Source: {source}");
            foreach (var stage in stages)
            {
                var mapped = new List<Range?>();
                foreach (var map in stage)
                {
                    var unmapped = new List<Range?>();
                    foreach (var sourceRange in source)
                    {
                        var (before, during, after) = map.Apply(sourceRange);
                        unmapped.Add(before);
                        mapped.Add(during);
                        unmapped.Add(after);
                    }

                    // After applying each map in a stage, we retry with unmapped ranges
                    source = unmapped.Where(x => x?.Length > 0).Select(x => x!).Distinct().ToList();
                }

                // After applying the whole stage, we continue with all mapped and unmapped
                source = source.Concat(mapped.Where(x => x?.Length > 0).Select(x => x!)).ToList();

                //Console.WriteLine("Stage complete");
                //foreach (var x in source)
                //{
                //    Console.WriteLine(x);
                //}
            }

            result = Math.Min(result, source.Min(r => r.From));

            Console.WriteLine(result);
        }
    }
}