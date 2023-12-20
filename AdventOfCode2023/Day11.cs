namespace AdventOfCode2023
{
    public class Day11 : Aoc
    {
        [TestCase(1)]
        [TestCase(2)]
        public void Part(int part)
        {
            var scale = part switch { 1 => 1, 2 => 999999 };

            var input = InputLines().ToList();
            var rows = new HashSet<int>();
            var cols = new HashSet<int>();
            for (var i = 0; i < input.Count; i++)
            {
                var line = input[i];
                for (var j = 0; j < input[i].Length; j++)
                {
                    if (line[j] == '#')
                    {
                        rows.Add(i);
                        cols.Add(j);
                    }
                }
            }

            var emptyRows = Enumerable.Range(0, input.Count).Except(rows).ToList();
            var emptyCols = Enumerable.Range(0, input[0].Length).Except(cols).ToList();

            var coords = new List<Coord>();
            for (var i = 0; i < input.Count; i++)
            {
                var line = input[i];
                for (var j = 0; j < line.Length; j++)
                {
                    if (line[j] == '#')
                    {
                        var x = i + scale * emptyRows.Count(ii => ii < i);
                        var y = j + scale * emptyCols.Count(jj => jj < j);
                        coords.Add(new Coord(x, y));
                    }
                }
            }

            long result = 0;
            for (var i = 0; i < coords.Count; i++)
            {
                var a = coords[i];
                for (var j = 0; j < i; j++)
                {
                    var b = coords[j];
                    result += Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
                }
            }

            Console.WriteLine(result);
        }
    }
}