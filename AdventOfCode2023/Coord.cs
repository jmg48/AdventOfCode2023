namespace AdventOfCode2023;

public record Coord(int X, int Y)
{
    public Coord Move(Dir dir, int dist = 1)
    {
        return dir switch
        {
            Dir.N => new Coord(this.X - dist, this.Y),
            Dir.S => new Coord(this.X + dist, this.Y),
            Dir.E => new Coord(this.X, this.Y + dist),
            Dir.W => new Coord(this.X, this.Y - dist),
        };
    }
}