namespace AdventOfCode2023
{
    public class Day14
    {
        [TestCase(1)]
        public void Part(int part)
        {
            var input = File.ReadLines("Input14.txt").ToList();

            foreach (var line in input)
            {
                Console.WriteLine(new string(line.ToArray()));
            }

            Console.WriteLine();

            var result = 0;
            for (var j = 0; j < input[0].Length; j++)
            {
                var col = new string( input.Select(row => row[j]).ToArray());
                var sorted = string.Join("#",
                    col.Split('#').Select(s => new string(s.OrderBy(c => c == '.').ToArray())));
                Console.WriteLine(sorted);
                var score = sorted.Select((c, i) => c == 'O' ? input.Count - i : 0).Sum();
                result += score;
            }

            Console.WriteLine(result);
        }
    }
}