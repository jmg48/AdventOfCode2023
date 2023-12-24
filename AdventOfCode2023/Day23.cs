using System.Collections.Concurrent;

namespace AdventOfCode2023
{
    public class Day23 : Aoc
    {
        [TestCase(1)]
        [TestCase(2)]
        public void Part(int part)
        {
            var input = InputLines().ToList();
            foreach (var inputLine in input)
            {
                //Console.WriteLine(inputLine);
            }

            var result = 0;

            var start = new Coord(0, 1);
            var dest = new Coord(input.Count - 1, input[0].Length - 2);

            var forks = new ConcurrentStack<((Coord, int), HashSet<Coord>)>();
            forks.Push(((start, 0), new HashSet<Coord>()));
            while (forks.TryPop(out var popped))
            {
                var work = new List<((Coord, int), HashSet<Coord>)> { popped };
                for (var i = 0; i < 256; i++)
                {
                    if (forks.TryPop(out var popped2))
                    {
                        work.Add(popped2);
                    }
                }

                var x = work.AsParallel().WithDegreeOfParallelism(32).Select(it =>
                {
                    var ((pos, dist), visited) = it;
                    while (true)
                    {
                        visited.Add(pos);

                        if (pos == dest)
                        {
                            Console.WriteLine($"Reached dest with path {dist}");
                            return dist;
                        }

                        var nexts = new List<(Coord, int)>();
                        foreach (var dir in new[] { Dir.N, Dir.S, Dir.E, Dir.W })
                        {
                            var next = pos.Move(dir);
                            if (next.X > 0 && next.Y >= 0 && next.X < input.Count && next.Y < input[0].Length && !visited.Contains(next))
                            {
                                if (part == 1)
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
                                else
                                {
                                    switch (input[(int)next.X][(int)next.Y])
                                    {
                                        case '#':
                                            break;
                                        case '.':
                                        case '>':
                                        case '^':
                                        case '<':
                                        case 'v':
                                            nexts.Add((next, dist + 1));
                                            break;
                                        default:
                                            throw new Exception();
                                    }
                                }
                            }
                        }

                        if (nexts.Count == 0)
                        {
                            Console.WriteLine($"Dead end at {pos} with path length {dist}");
                            return -1;
                        }

                        switch (nexts.Count)
                        {
                            case 1:
                                (pos, dist) = nexts[0];
                                break;
                            case 2 or 3 or 4:
                                //Console.WriteLine($"Fork at {pos} with path length {dist}");
                                (pos, dist) = nexts[0];
                                for (int i = 1; i < nexts.Count; i++)
                                {
                                    forks.Push((nexts[i], new HashSet<Coord>(visited)));
                                }

                                break;
                        }
                    }
                }).Max();
                if (x > result)
                {
                    result = x;
                    File.AppendAllLines("C:\\src\\23.txt", new[] { result.ToString() });
                }
            }

            Console.WriteLine(result);
        }
    }
}