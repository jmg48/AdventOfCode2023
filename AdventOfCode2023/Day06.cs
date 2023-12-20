using System.Diagnostics;

namespace AdventOfCode2023
{
    public class Day06 : Aoc
    {
        [Test]
        public void Part1()
        {
            var input = InputLines().ToArray();
            var times = input[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
            var distances = input[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();

            var result = 1;
            for (var i = 0; i < times.Count; i++)
            {
                var wins = 0;

                for (var t = 0; t < times[i]; t++)
                {
                    var speed = t;
                    var distance = speed * (times[i] - t);
                    if (distance > distances[i])
                    {
                        wins++;
                    }
                }

                result *= wins;
            }

            Console.WriteLine(result);
        }

        [Test]
        public void Part2()
        {
            var timer = new Stopwatch();
            timer.Start();

            long time = 48938466;
            long record = 261119210191063;
            long bigStep = 1000000;

            long minWin = 0;
            var increment = bigStep;
            for (long t = 0; t < time; t+= increment)
            {
                var speed = t;
                var distance = speed * (time - t);
                if (distance > record)
                {
                    if (increment > 1)
                    {
                        t -= increment;
                        increment /= 10;
                    }
                    else
                    {
                        minWin = t;
                        break;
                    }
                }
            }

            long minLoss = 0;
            increment = bigStep;
            for (long t = minWin; t < time; t += increment)
            {
                var speed = t;
                var distance = speed * (time - t);
                if (distance <= record)
                {
                    if (increment > 1)
                    {
                        t -= increment;
                        increment /= 10;
                    }
                    else
                    {
                        minLoss = t;
                        break;
                    }
                }
            }

            timer.Stop();

            Console.WriteLine(minLoss - minWin);
            Console.WriteLine($"{timer.ElapsedMilliseconds}ms");
        }
    }
}