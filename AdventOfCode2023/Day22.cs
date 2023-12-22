using System.Diagnostics;
using System.Reflection.Emit;

namespace AdventOfCode2023
{
    public class Day22 : Aoc
    {
        private record Brick(int X1, int Y1, int Z1, int X2, int Y2, int Z2);

        [Test]
        public async Task Part()
        {
            var timer = new Stopwatch();
            timer.Restart();

            var bricks = new Dictionary<int, List<Brick>>();
            foreach (var line in InputLines())
            {
                var split = line.Split('~', ',').Select(int.Parse).ToList();
                var brick = new Brick(split[0], split[1], split[2], split[3], split[4], split[5]);
                AddBrick(bricks, brick);
            }

            Collapse(bricks);

            var supportMap = new HashSet<(Brick Below, Brick Above)>();
            foreach (var brick in bricks.SelectMany(kvp => kvp.Value))
            {
                if (brick.Z1 == 1)
                {
                    continue;
                }

                var layerBelow = bricks[brick.Z1 - 1];
                foreach (var brickBelow in layerBelow)
                {
                    for (var i = brick.X1; i <= brick.X2; i++)
                    {
                        for (var j = brick.Y1; j <= brick.Y2; j++)
                        {
                            if (brickBelow.X1 <= i && i <= brickBelow.X2 && brickBelow.Y1 <= j && j <= brickBelow.Y2)
                            {
                                supportMap.Add((brickBelow, brick));
                            }
                        }
                    }
                }
            }

            var criticalBricks = supportMap
                .GroupBy(it => it.Above)
                .Where(grouping => grouping.Count() == 1)
                .Select(grouping => grouping.Single().Below)
                .Distinct()
                .ToList();
            
            var safe = bricks.SelectMany(layer => layer.Value).Except(criticalBricks).ToList();
            Console.WriteLine($"Part 1: {safe.Count} in {timer.ElapsedMilliseconds}ms");
            timer.Restart();

            var result = await Task.WhenAll(bricks.Values.SelectMany(layer => layer).Select(brick =>
                Task.Factory.StartNew(() =>
                {
                    var copy = bricks.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToList());
                    Assert.That(copy[brick.Z2].Remove(brick), Is.True);
                    return Collapse(copy);
                })));

            Console.WriteLine($"Part 2: {result.Sum()} in {timer.ElapsedMilliseconds}ms");
        }

        private int Collapse(Dictionary<int, List<Brick>> bricks)
        {
            var result = 0;

            var loToHi = bricks.OrderBy(kvp => kvp.Key).SelectMany(kvp => kvp.Value).ToList();
            for (var index = 0; index < loToHi.Count; index++)
            {
                var brick = loToHi[index];
                var fell = false;
                while (brick.Z1 > 1)
                {
                    var isSupported = false;
                    if (bricks.TryGetValue(brick.Z1 - 1, out var layerBelow))
                    {
                        foreach (var brickBelow in layerBelow)
                        {
                            for (var i = brick.X1; i <= brick.X2 && !isSupported; i++)
                            {
                                for (var j = brick.Y1; j <= brick.Y2 && !isSupported; j++)
                                {
                                    if (brickBelow.X1 <= i && i <= brickBelow.X2 && brickBelow.Y1 <= j && j <= brickBelow.Y2)
                                    {
                                        isSupported = true;
                                    }
                                }
                            }
                        }
                    }

                    if (isSupported)
                    {
                        break;
                    }

                    Assert.That(bricks[brick.Z2].Remove(brick), Is.True);
                    brick = brick with { Z1 = brick.Z1 - 1, Z2 = brick.Z2 - 1 };
                    AddBrick(bricks, brick);
                    fell = true;
                }

                if (fell)
                {
                    result++;
                }
            }

            return result;
        }

        void AddBrick(Dictionary<int, List<Brick>> bricks, Brick brick)
        {
            if (!bricks.TryGetValue(brick.Z2, out var layer))
            {
                layer = new List<Brick>();
                bricks.Add(brick.Z2, layer);
            }

            layer.Add(brick);
        }
    }
}