using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Serialization;
using NUnit.Framework.Constraints;

namespace AdventOfCode2023
{
    public class Day16
    {
        [Test]
        public void Part1()
        {
            var input = File.ReadLines("Input16.txt").ToList();

            var work = new Stack<(Coord Pos, Dir Dir)>();
            work.Push((new Coord(0, -1), Dir.E));

            var history = new HashSet<(Coord Pos, Dir Dir)>();

            while (work.TryPop(out var current))
            {
                if (history.Contains(current))
                {
                    Console.WriteLine($"Cycle at {current.Pos}, {current.Dir}");
                    continue;
                }

                history.Add(current);

                var next = current.Pos.Move(current.Dir);
                if (next.X < 0 || next.Y < 0 || next.X >= input.Count || next.Y >= input[0].Length)
                {
                    Console.WriteLine($"Out of bounds at {next}, {current.Dir}");
                    continue;
                }

                switch ((input[next.X][next.Y], current.Dir))
                {
                    case ('.', _):
                    case ('|', Dir.N):
                    case ('|', Dir.S):
                    case ('-', Dir.E):
                    case ('-', Dir.W):
                        work.Push((next, current.Dir));
                        break;
                    case ('|', Dir.E):
                    case ('|', Dir.W):
                        work.Push((next, Dir.N));
                        work.Push((next, Dir.S));
                        break;
                    case ('-', Dir.N):
                    case ('-', Dir.S):
                        work.Push((next, Dir.E));
                        work.Push((next, Dir.W));
                        break;
                    case ('/', Dir.N):
                        work.Push((next, Dir.E));
                        break;
                    case ('\\', Dir.N):
                        work.Push((next, Dir.W));
                        break;
                    case ('/', Dir.S):
                        work.Push((next, Dir.W));
                        break;
                    case ('\\', Dir.S):
                        work.Push((next, Dir.E));
                        break;
                    case ('/', Dir.E):
                        work.Push((next, Dir.N));
                        break;
                    case ('\\', Dir.E):
                        work.Push((next, Dir.S));
                        break;
                    case ('/', Dir.W):
                        work.Push((next, Dir.S));
                        break;
                    case ('\\', Dir.W):
                        work.Push((next, Dir.N));
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            var result = 0;
            for (int i = 0; i < input.Count; i++)
            {
                for (int j = 0; j < input[0].Length; j++)
                {
                    var dirs = new [] { Dir.N, Dir.S, Dir.E, Dir.W}.Where(it => history.Contains((new Coord(i, j), it))).ToList();
                    Console.Write(dirs.Count switch
                    {
                        0 => input[i][j],
                        1 => dirs[0] switch { Dir.N => '^', Dir.S => 'v', Dir.E => '>', Dir.W => '<' },
                        2 => '2',
                        3 => '3',
                        4 => '4',
                    });

                    result += dirs.Count > 0 ? 1 : 0;
                }

                Console.WriteLine();
            }

            Console.WriteLine(result);
        }
    }
}