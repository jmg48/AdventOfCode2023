using System.Security.Cryptography;

namespace AdventOfCode2023
{
    public class Day18
    {
        [Test]
        public void Part1()
        {
            var input = File.ReadLines("Input18a.txt").ToList();

            var map = new Dictionary<Coord, string>();

            {
                var pos = new Coord(0, 0);
                foreach (var line in input)
                {
                    var split = line.Split(' ');

                    var xxx = Convert.ToInt32(split[2][2..^2], 16);

                    var dir = split[0] switch
                    {
                        "U" => Dir.N,
                        "D" => Dir.S,
                        "R" => Dir.E,
                        "L" => Dir.W,
                    };

                    var dist = int.Parse(split[1]);

                    map[pos] = split[2];
                    for (int i = 0; i < dist; i++)
                    {
                        pos = pos.Move(dir);
                        map[pos] = split[2];
                    }
                }
            }

            var xMin = map.Keys.Select(it => it.X).Min();
            var xMax = map.Keys.Select(it => it.X).Max();
            var yMin = map.Keys.Select(it => it.Y).Min();
            var yMax = map.Keys.Select(it => it.Y).Max();

            var outside = new HashSet<Coord>();
            var fill = new Queue<Coord>();
            fill.Enqueue(new Coord(xMin - 1, yMin - 1));
            while (fill.TryDequeue(out var pos))
            {
                if (pos.X < xMin - 1 || pos.Y < yMin - 1 || pos.X > xMax + 1 || pos.Y > yMax + 1)
                {
                    continue;
                }

                if (map.ContainsKey(pos) || outside.Contains(pos))
                {
                    continue;
                }

                outside.Add(pos);

                foreach (var dir in new[] { Dir.N, Dir.S, Dir.E, Dir.W})
                {
                    fill.Enqueue(pos.Move(dir));
                }
            }

            var result = 0;
            for (int i = xMin; i <= xMax; i++)
            {
                var crossings = 0;
                for (int j = yMin; j <= yMax; j++)
                {
                    if (map.TryGetValue(new Coord(i, j), out var val))
                    {
                        result++;
                        Console.Write("#");
                    }
                    else if(outside.Contains(new Coord(i, j)))
                    {
                        Console.Write(".");
                    }
                    else
                    {
                        result++;
                        Console.Write("o");
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine(result);
        }
    }
}