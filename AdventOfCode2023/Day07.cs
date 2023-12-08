namespace AdventOfCode2023
{
    public class Day07
    {
        [TestCase(1)]
        [TestCase(2)]
        public void Part(int part)
        {
            var input = File.ReadAllLines("Input7.txt");

            var result = input.Select(line =>
                {
                    var split = line.Split(' ');
                    var hand = new Hand(split[0].Select(c => new Card(c, part)).ToList());
                    return (hand, bid: int.Parse(split[1]));
                }).OrderBy(x => x.hand)
                .Select((x, i) => x.bid * (i + 1))
                .Sum();

            Console.WriteLine(result);
        }
    }
}