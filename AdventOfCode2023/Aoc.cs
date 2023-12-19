using System.Text.RegularExpressions;

namespace AdventOfCode2023;

public class Aoc
{
    public string InputText() => Input(File.ReadAllText);

    public IEnumerable<string> InputLines() => Input(File.ReadLines);

    private T Input<T>(Func<string, T> f)
    {
        var day = int.Parse(Regex.Match(this.GetType().Name, @"\D+(\d+)").Groups[1].Value);
        var inputFile = $@"C:\src\AdventOfCode2023\AdventOfCode2023\input{day}.txt";
        var sessionToken = File.ReadAllText(@"C:\src\aocSession.txt");

        var utcPuzzleDate = new DateTime(2023, 12, day, 5, 0, 0);
        if (DateTime.UtcNow < utcPuzzleDate)
        {
            throw new TooEarlyException();
        }

        if (!File.Exists(inputFile))
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Cookie", $"session={sessionToken}");
            var input = client.GetStringAsync($"https://adventofcode.com/2023/day/{day}/input").Result;
            File.WriteAllText(inputFile, input);
        }

        return f(inputFile);
    }
}