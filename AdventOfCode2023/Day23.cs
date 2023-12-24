using System.Diagnostics;
using System.Reflection.Emit;
using Newtonsoft.Json.Serialization;
using NUnit.Framework.Constraints;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode2023
{
    public class Day23 : Aoc
    {
        [Test]
        public async Task Part()
        {
            var input = InputLines().ToList();
            foreach (var inputLine in input)
            {
                //Console.WriteLine(inputLine);
            }

            var start = new Coord(0, 1);
            var dest = new Coord(input.Count - 1, input[0].Length - 2);

            var (pos, dist) = (start, 0);
            var visited = new HashSet<Coord>();
            while (true)
            {
                visited.Add(pos);

                if (pos == dest)
                {
                    Console.WriteLine($"Reached dest with path {dist}");
                    break;
                }

                var nexts = new List<(Coord, int)>();
                foreach (var dir in new[] { Dir.N, Dir.S, Dir.E, Dir.W })
                {
                    var next = pos.Move(dir);
                    if (next.X > 0 && next.Y >= 0 && next.X < input.Count && next.Y < input[0].Length && !visited.Contains(next))
                    {
                        switch (input[(int)next.X][(int)next.Y])
                        {
                            case '#':
                                break;
                            case '.':
                                nexts.Add((next, dist + 1));
                                break;
                            case '>':
                                next = next.Move(Dir.E);
                                if (next != pos)
                                {
                                    nexts.Add((next, dist + 2));
                                }

                                break;
                            case '^':
                                next = next.Move(Dir.N);
                                if (next != pos)
                                {
                                    nexts.Add((next, dist + 2));
                                }

                                break;
                            case '<':
                                next = next.Move(Dir.W);
                                if (next != pos)
                                {
                                    nexts.Add((next, dist + 2));
                                }

                                break;
                            case 'v':
                                next = next.Move(Dir.S);
                                if (next != pos)
                                {
                                    nexts.Add((next, dist + 2));
                                }

                                break;
                            default:
                                throw new Exception();
                        }
                    }
                }

                switch (nexts.Count)
                {
                    case 0:
                        Console.WriteLine($"Dead end at {pos} with path length {dist}");
                        break;
                    case 1:
                        (pos, dist) = nexts[0];
                        break;
                    case 2:
                    case 3:
                    case 4:
                        Console.WriteLine($"Fork at {pos} with path length {dist}");
                        (pos, dist) = nexts[0];
                        break;
                }
            }
        }
    }
}