namespace AdventOfCode2023
{
    public class Day13 : Aoc
    {
        [TestCase(1)]
        [TestCase(2)]
        public void Part(int part)
        {
            var target = part switch { 1 => 0, 2 => 1 };

            var patterns = new List<List<string>> { new() };
            foreach (var line in InputLines())
            {
                if (line.Length == 0)
                {
                    patterns.Add(new List<string>());
                }
                else
                {
                    patterns[^1].Add(line);
                }
            }

            var result = 0;
            foreach (var pattern in patterns)
            {
                // Rows and cols are similar, just need to know the number of rows / cols and how to access a col / row in each case
                foreach (var (colsOrRows, scoreMultiple, rowOrColSelector) in
                         new (int, int, Func<int, IEnumerable<char>>)[]
                         {
                             (pattern.Count, 100, it => pattern[it]),
                             (pattern[0].Length, 1, it => pattern.Select(row => row[it])),
                         })
                {
                    for (var i = 1; i < colsOrRows; i++)
                    {
                        var smudges = 0;
                        for (var (j, k) = (i, i - 1); j < colsOrRows && k >= 0; (j, k) = (j + 1, k - 1))
                        {
                            smudges += rowOrColSelector(j).Zip(rowOrColSelector(k)).Count(it => it.First != it.Second);
                        }

                        result += smudges == target ? scoreMultiple * i : 0;
                    }
                }
            }

            Console.WriteLine(result);
        }
    }
}