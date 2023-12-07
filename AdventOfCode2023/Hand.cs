namespace AdventOfCode2023;

public record Hand(List<Card> Cards) : IComparable<Hand>
{
    // Returns a two digit number representing the significant scoring groups, e.g. a full house is 32
    public int Type
    {
        get
        {
            var jokers = Cards.Count(c => c.Strength < 0);
            var groupSizes = Cards
                .Where(c => c.Strength >= 0)
                .GroupBy(c => c)
                .Select(group => group.Count())
                .OrderByDescending(i => i)
                .ToList();

            return (groupSizes.Count < 1 ? 0 : groupSizes[0], groupSizes.Count < 2 ? 0 : groupSizes[1], jokers) switch
            {
                (_, _, 5) => 50,
                (5, _, _) => 50,
                (4, _, 1) => 50,
                (4, _, _) => 41,
                (3, _, 2) => 50,
                (3, _, 1) => 41,
                (3, 2, _) => 32,
                (3, _, _) => 31,
                (2, _, 3) => 50,
                (2, _, 2) => 41,
                (2, 2, 1) => 32,
                (2, _, 1) => 31,
                (2, 2, _) => 22,
                (2, _, _) => 21,
                (_, _, 4) => 50,
                (_, _, 3) => 41,
                (_, _, 2) => 31,
                (_, _, 1) => 21,
                (_, _, _) => 11,
            };
        }
    }

    public int CompareTo(Hand? other)
    {
        var compareType = this.Type.CompareTo(other.Type);
        if (compareType != 0)
        {
            return compareType;
        }

        for (var i = 0; i < 5; i++)
        {
            var compareCard = this.Cards[i].CompareTo(other.Cards[i]);
            if (compareCard != 0)
            {
                return compareCard;
            }
        }

        return 0;
    }

    public override string ToString() => string.Concat(Cards.Select(c => c.ToString()));
}