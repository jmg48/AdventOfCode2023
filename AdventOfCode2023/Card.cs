namespace AdventOfCode2023;

public record Card(char C, int Part) : IComparable<Card>
{
    public int Strength => C switch
    {
        '2' => 0,
        '3' => 1,
        '4' => 2,
        '5' => 3,
        '6' => 4,
        '7' => 5,
        '8' => 6,
        '9' => 7,
        'T' => 8,
        'J' => Part == 1 ? 9 : -1,
        'Q' => 10,
        'K' => 11,
        'A' => 12,
        _ => throw new ArgumentOutOfRangeException()
    };

    public int CompareTo(Card? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return this.Strength.CompareTo(other.Strength);
    }

    public override string ToString() => C.ToString();
}