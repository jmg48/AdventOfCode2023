using System.Collections.Concurrent;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using NUnit.Framework.Internal.Commands;

namespace AdventOfCode2023
{
    public record Range(long From, long Length)
    {
        public long To => this.From + this.Length;

        public Range Intersect(Range range)
        {
            var from1 = this.From;
            var from2 = range.From;
            var to1 = this.From + this.Length;
            var to2 = range.From + range.Length;

            var from = Math.Max(from1, from2);
            var to = Math.Min(to1, to2);

            if (to > from)
            {
                return new Range(from, to - from);
            }

            return new Range(0, 0);
        }
    }

    public record Map(long DestStart, long SourceStart, long Length)
    {
        public bool IsMatch(long sourceValue)
        {
            return sourceValue >= SourceStart && (sourceValue - SourceStart) < Length;
        }

        public long DestValue(long sourceValue)
        {
            checked
            {
                if (!IsMatch(sourceValue))
                {
                    return sourceValue;
                }

                return sourceValue - SourceStart + DestStart;
            }
        }

        public Range From()
        {
            return new Range(SourceStart, Length);
        }

        public (Range? before, Range? during, Range? after) Apply(Range input)
        {
            checked
            {
                Range mapped = new Range(this.SourceStart, this.Length);

                if (input.To < mapped.From)
                {
                    return (input, null, null);
                }

                if (input.From > mapped.To)
                {
                    return (null, null, input);
                }

                // So now we know they overlap

                Range? before;
                if (input.From < mapped.From)
                {
                    before = new Range(input.From, mapped.From - input.From);
                }
                else
                {
                    before = null;
                }

                Range? after;
                if (input.To > mapped.To)
                {
                    after = new Range(mapped.To, input.To - mapped.To);
                }
                else
                {
                    after = null;
                }

                Range? during;
                if (input.From < mapped.From)
                {
                    if (input.To > mapped.To)
                    {
                        during = mapped;
                    }
                    else
                    {
                        during = new Range(mapped.From, input.To - mapped.From);
                    }
                }
                else
                {
                    if (input.To > mapped.To)
                    {
                        during = new Range(input.From, mapped.To - input.From);
                    }
                    else
                    {
                        during = new Range(input.From, input.To - input.From);
                    }
                }

                var duringMapped = new Range(during.From - this.SourceStart + this.DestStart, during.Length);

                return (before, duringMapped, after);
            }
        }
    }

    public class Day05
    {
        [TestCase(1)]
        [TestCase(2)]
        public void Part2(int part)
        {
            var input = File.ReadAllLines("Input5.txt");

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