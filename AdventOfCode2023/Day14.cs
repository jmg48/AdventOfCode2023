namespace AdventOfCode2023
{
    public class Day14
    {
        [Test]
        public void Part1()
        {
            var input = File.ReadLines("Input14.txt").ToList();

            var result = 0;
            for (var j = 0; j < input[0].Length; j++)
            {
                var col = new string( input.Select(row => row[j]).ToArray());
                var sorted = string.Join("#",
                    col.Split('#').Select(s => new string(s.OrderBy(c => c == '.').ToArray())));
                var score = sorted.Select((c, i) => c == 'O' ? input.Count - i : 0).Sum();
                result += score;
            }

            Console.WriteLine(result);
        }

        [Test]
        public void Part2()
        {
            var input = File.ReadLines("Input14.txt").Select(s => s.ToList()).ToList();

            var loads = new List<int>();
            for (var cycle = 1; cycle <= 1000; cycle++)
            {
                for (var dir = 0; dir < 4; dir++)
                {
                    var next = Enumerable.Range(0, input.Count).Select(i => new List<char>()).ToList();
                    for (var j = 0; j < input[0].Count; j++)
                    {
                        var col = new string(input.Select(row => row[j]).ToArray());
                        var sorted = string.Join("#",
                            col.Split('#').Select(s => new string(s.OrderBy(c => c == '.').ToArray())));

                        for (var i = input.Count - 1; i >= 0; i--)
                        {
                            next[j].Add(sorted[i]);
                        }
                    }

                    input = next;
                }

                var load = input.Select((row, i) => row.Count(c => c == 'O') * (input.Count - i)).Sum();
                loads.Add(load);

                if (TryFindCycle(loads, out var cycleLength))
                {
                    var totalCycles = 1000000000;
                    while (totalCycles > loads.Count)
                    {
                        totalCycles -= cycleLength;
                    }

                    Console.WriteLine(loads[totalCycles - 1]);
                    return;
                }
            }
        }

        private static bool TryFindCycle(List<int> loads, out int cycleLength)
        {
            // Floyd's tortoise and hare:
            // for all i, check if f(2i) == f(i)
            // i.e. check over a gap that increases by 1 each time, so the gap much eventually either be the cycle length or a multiple of it
            // This method is called each time you add an item to the list, so you can terminate generation of the list early if a cycle is found
            var item = loads[^1];
            if (loads.Count % 2 != 0)
            {
                cycleLength = 0;
                return false;
            }

            if (loads[loads.Count / 2 - 1] != item)
            {
                cycleLength = 0;
                return false;
            }

            Console.WriteLine($"Repeat detected at {loads.Count / 2}");
            for (var i = loads.Count / 2; i < loads.Count; i++)
            {
                if (loads[i] != item)
                {
                    continue;
                }

                // Once a repeat is detected, look for possible cycle lengths within that window
                cycleLength = i - (loads.Count / 2 - 1);
                Console.WriteLine($"Possible cycle length {cycleLength}");
                
                // Then check if the cycle length is valid
                var validCycle = true;
                for (var j = loads.Count / 2 - 1; j < loads.Count - cycleLength; j++)
                {
                    if (loads[j] != loads[j + cycleLength])
                    {
                        validCycle = false;
                        break;
                    }
                }

                if (validCycle)
                {
                    return true;
                }
            }

            cycleLength = 0;
            return false;
        }
    }
}