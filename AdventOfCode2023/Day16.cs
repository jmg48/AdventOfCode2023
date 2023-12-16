namespace AdventOfCode2023
{
    public class Day16
    {
        [Test]
        public void Part1()
        {
            var input = File.ReadLines("Input16.txt").ToList();
            Console.WriteLine(Energy(input, (new Coord(0, -1), Dir.E)));
        }

        [Test]
        public void Part2()
        {
            var input = File.ReadLines("Input16.txt").ToList();

            var result = 0;
            for (var i = 0; i < input.Count; i++)
            {
                var energy1 = Energy(input, (new Coord(i, -1), Dir.E));
                var energy2 = Energy(input, (new Coord(i, input[0].Length), Dir.W));

                result = Math.Max(result, Math.Max(energy1, energy2));
            }

            for (var j = 0; j < input[0].Length; j++)
            {
                var energy1 = Energy(input, (new Coord(-1, j), Dir.S));
                var energy2 = Energy(input, (new Coord(input.Count, j), Dir.N));

                result = Math.Max(result, Math.Max(energy1, energy2));
            }

            Console.WriteLine(result);
        }

        private static int Energy(List<string> input, (Coord, Dir) start)
        {
            var work = new Stack<(Coord Pos, Dir Dir)>();
            work.Push(start);

            var history = new HashSet<(Coord Pos, Dir Dir)>();
            var result = new HashSet<Coord>();

            while (work.TryPop(out var current))
            {
                if (history.Contains(current))
                {
                    continue;
                }

                history.Add(current);

                var next = current.Pos.Move(current.Dir);
                if (next.X < 0 || next.Y < 0 || next.X >= input.Count || next.Y >= input[0].Length)
                {
                    continue;
                }

                result.Add(next);

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

            return result.Count;
        }
    }
}