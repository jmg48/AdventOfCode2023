using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Serialization;

namespace AdventOfCode2023
{
    public class Day15
    {
        [Test]
        public void Part1() => Console.WriteLine(File.ReadAllText("Input15.txt").Split(',').Select(Hash).Sum());

        [Test]
        public void Part2()
        {
            var input = File.ReadLines("Input15.txt").ToList();

            var steps = input[0].Split(',');

            var boxes = Enumerable.Range(0, 256).Select(i => new List<(string Label, int Length)>()).ToList();
            foreach (var step in steps)
            {
                var match = Regex.Match(step, @"(\w+)(-|=)(\d*)");
                var label = match.Groups[1].Value;
                var box = boxes[Hash(label)];
                switch (match.Groups[2].Value)
                {
                    case "-":
                        for (var i = 0; i < box.Count; i++)
                        {
                            if (box[i].Label == label)
                            {
                                box.RemoveAt(i);
                                break;
                            }
                        }

                        break;
                    case "=":
                        var found = false;
                        var lens = (label, int.Parse(match.Groups[3].Value));
                        for (var i = 0; i < box.Count; i++)
                        {
                            if (box[i].Label == label)
                            {
                                box[i] = lens;
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            box.Add(lens);
                        }

                        break;
                }
            }

            var result = boxes.Select(
                    (box, i) => box.Select(
                            (lens, j) => (i + 1) * (j + 1) * lens.Length)
                        .Sum())
                .Sum();

            Console.WriteLine(result);
        }

        private static int Hash(string step) =>
            Encoding.UTF8.GetBytes(step).Aggregate(0, (hash, c) => ((hash + c) * 17) % 256);
    }
}