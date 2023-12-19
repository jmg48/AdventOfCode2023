using System.Security.Cryptography;

namespace AdventOfCode2023
{
    public class Day18
    {
        [TestCase(1)]
        [TestCase(2)]
        public void Part(int part)
        {
            var input = File.ReadLines("Input18.txt").ToList();

            var current = new Coord(0, 0);
            var pathLength = 0;
            long area = 0;
            foreach (var step in input.Select(line => line.Split(' ')))
            {
                var dir = part switch
                {
                    1 => step[0] switch { "U" => Dir.N, "D" => Dir.S, "R" => Dir.E, "L" => Dir.W, },
                    2 => step[2][^2] switch { '0' => Dir.E, '1' => Dir.S, '2' => Dir.W, '3' => Dir.N, },
                };

                var dist = part switch
                {
                    1 => int.Parse(step[1]),
                    2 => Convert.ToInt32(step[2][2..^2], 16),
                };

                var next = current.Move(dir, dist);

                pathLength += dist;
                
                // https://en.wikipedia.org/wiki/Shoelace_formula
                var det = ((long)current.X * next.Y) - ((long)next.X * current.Y);
                area += det;

                current = next;
            }

            // The edge of the calculated area follows the center of the outline trench, so we add:
            // 1) Half the path length, i.e. from the center of the outline trench to its edge
            // 2) One extra unit, i.e the four quarter units of the outer corners of the trench
            Console.WriteLine(Math.Abs(area / 2) + pathLength / 2 + 1);
        }
    }
}