using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public class Day10
    {
        [Test]
        public void Part()
        {
            var input = File.ReadAllLines("Input10.txt");

            Pipe Cell(Coord coord)
            {
                return new Pipe(input[coord.X][coord.Y]);
            }

            var distance = 0;
            var start = new Coord(0, 0);
            for (var i = 0; i < input.Length; i++)
            {
                var indexOf = input[i].IndexOf('S');
                if (indexOf >= 0)
                {
                    start = new Coord(i, indexOf);
                }
            }

            var position = new Coord(0, 0);
            var arrivedFrom = Dir.N;
            foreach (var dir in new[] { Dir.N, Dir.S, Dir.E, Dir.W })
            {
                var newPosition = start.Move(dir);
                var dirs = Cell(newPosition).Dirs;
                var inverse = Inverse(dir);
                if (inverse == dirs.Item1 || inverse == dirs.Item2)
                {
                    distance++;
                    position = newPosition;
                    arrivedFrom = inverse;
                    break;
                }
            }

            var path = new HashSet<Coord> { start };
            while (position != start)
            {
                path.Add(position);
                var dirs = Cell(position).Dirs;
                if (arrivedFrom == dirs.Item1)
                {
                    position = position.Move(dirs.Item2);
                    arrivedFrom = Inverse(dirs.Item2);
                }
                else
                {
                    position = position.Move(dirs.Item1);
                    arrivedFrom = Inverse(dirs.Item1);
                }

                distance++;
            }

            Console.WriteLine(distance / 2);

            var result2 = 0;
            for (var i = 0; i < input.Length; i++)
            {
                var isInside = false;
                for (var j = 0; j < input[0].Length; j++)
                {
                    if (path.Contains(new Coord(i, j)))
                    {
                        var current = input[i][j];
                        Console.Write(current);
                        switch (current)
                        {
                            case 'S': // because S is F in this puzzle
                            case 'F':
                            case '7':
                            case '|':
                                isInside = !isInside;
                                break;
                        }
                    }
                    else if (isInside)
                    {
                        Console.Write("i");
                        result2++;
                    }
                    else
                    {
                        Console.Write("o");
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine(result2);
        }

        private Dir Inverse(Dir dir)
        {
            return dir switch
            {
                Dir.N => Dir.S,
                Dir.S => Dir.N,
                Dir.E => Dir.W,
                Dir.W => Dir.E,
            };
        }
    }

    public record Pipe(char symbol)
    {
        public (Dir, Dir) Dirs = symbol switch
        {
            '|' => (Dir.N, Dir.S),
            '-' => (Dir.E, Dir.W),
            'L' => (Dir.N, Dir.E),
            'J' => (Dir.N, Dir.W),
            '7' => (Dir.S, Dir.W),
            'F' => (Dir.S, Dir.E),
        };
    }

    public record Coord(int X, int Y)
    {
        public Coord Move(Dir dir)
        {
            return dir switch
            {
                Dir.N => new Coord(this.X - 1, this.Y),
                Dir.S => new Coord(this.X + 1, this.Y),
                Dir.E => new Coord(this.X, this.Y + 1),
                Dir.W => new Coord(this.X, this.Y - 1),
            };
        }
    }

    public enum Dir
    {
        N, S, E, W
    }
}