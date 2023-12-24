using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection.Emit;

namespace AdventOfCode2023
{
    public class Day24 : Aoc
    {
        [Test]
        public async Task Part()
        {
            var hailStones = new List<(Coord position, Coord velocity)>();
            var inputLines = InputLines();
//            inputLines = @"19, 13, 30 @ -2,  1, -2
//18, 19, 22 @ -1, -1, -2
//20, 25, 34 @ -2, -2, -4
//12, 31, 28 @ -1, -2, -1
//20, 19, 15 @  1, -5, -3".Split("\n");
            foreach (var inputLine in inputLines)
            {
                var split = inputLine.Split(',', '@').Select(s => s.Trim()).Select(long.Parse).ToArray();
                hailStones.Add((new Coord(split[0], split[1]), new Coord(split[3], split[4])));
            }

            var result = new List<(int, int, double, double, double, double)>();
            for (var i = 0; i < hailStones.Count; i++)
            {
                var left = hailStones[i];
                var a1 = (double)left.velocity.X;
                var b1 = (double)left.position.X;
                var c1 = (double)left.velocity.Y;
                var d1 = (double)left.position.Y;
                for (var j = 0; j < i; j++)
                {
                    var right = hailStones[j];
                    var a2 = (double)right.velocity.X;
                    var b2 = (double)right.position.X;
                    var c2 = (double)right.velocity.Y;
                    var d2 = (double)right.position.Y;

                    var x = (a2 * c1 * b1 - a1 * c2 * b2 + a1 * a2 * (d2 - d1)) / (a2 * c1 - a1 * c2);

                    var t1 = (x - b1) / a1;
                    var t2 = (x - b2) / a2;

                    var y = c1 * t1 + d1;

                    var lo = 200000000000000;
                    var hi = 400000000000000;
                    //(lo, hi) = (7, 27);
                    if (t1 >= 0 && t2>= 0 && x >= lo && y >= lo && x <= hi && y <= hi)
                    {
                        //Console.WriteLine((i,j,t1, t2, x, y));
                        result.Add((i,j,t1,t2, x, y));
                    }
                }
            }

            Console.WriteLine(result.Count);
        }
    }
}