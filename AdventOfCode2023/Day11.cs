using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public class Day11
    {
        [Test]
        public void Part()
        {
            var input = File.ReadLines("Input11.txt").ToList();
            var cols = new HashSet<int>();
            for (int i = 0; i < input.Count; i++)
            {
                var line = input[i];
                var found = false;
                for (int j = 0; j < input[i].Length; j++)
                {
                    if (line[j] == '#')
                    {
                        cols.Add(j);
                        found = true;
                    }
                }

                if (!found)
                {
                    input.Insert(i, new string(input[i]));
                    i++;
                }
            }

            var lines = input.Select(s => s.ToList()).ToList();
            foreach (var col in Enumerable.Range(0, input[0].Length).Except(cols).OrderByDescending(i => i))
            {
                foreach (var line in lines)
                {
                    line.Insert(col, '.');
                }
            }

            var coords = new List<Coord>();
            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                for (int j = 0; j < line.Count; j++)
                {
                    if (line[j] == '#')
                    {
                        coords.Add(new Coord(i, j));
                    }
                }
            }

            long result = 0;
            for (int i = 0; i < coords.Count; i++)
            {
                var a = coords[i];
                for (int j = 0; j < i; j++)
                {
                    var b = coords[j];
                    result += Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
                }
            }

            Console.WriteLine(result);
        }
    }
}