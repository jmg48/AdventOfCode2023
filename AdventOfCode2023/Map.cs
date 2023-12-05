namespace AdventOfCode2023;

public record Map(long DestStart, long SourceStart, long Length)
{
    public (Range? before, Range? during, Range? after) Apply(Range input)
    {
        checked
        {
            var mapped = new Range(this.SourceStart, this.Length);

            if (input.To < mapped.From)
            {
                return (input, null, null);
            }

            if (input.From > mapped.To)
            {
                return (null, null, input);
            }

            // So now we know they overlap

            var before = input.From < mapped.From
                ? new Range(input.From, mapped.From - input.From)
                : null;

            var after = input.To > mapped.To
                ? new Range(mapped.To, input.To - mapped.To)
                : null;

            var duringFrom = Math.Max(input.From, mapped.From);
            var duringTo = Math.Min(input.To, mapped.To);
            var during = new Range(duringFrom, duringTo - duringFrom);

            var duringMapped = new Range(during.From - this.SourceStart + this.DestStart, during.Length);

            return (before, duringMapped, after);
        }
    }
}