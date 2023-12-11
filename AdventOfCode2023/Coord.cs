namespace AdventOfCode2023;

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