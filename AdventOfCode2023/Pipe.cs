namespace AdventOfCode2023;

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