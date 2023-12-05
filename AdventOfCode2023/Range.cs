namespace AdventOfCode2023;

public record Range(long From, long Length)
{
    public long To => this.From + this.Length;
}