using System.Security.Cryptography;

namespace AdventOfCode2023
{
    public class Day17
    {
        [TestCase(1)]
        [TestCase(2)]
        public void Part(int part)
        {
            var maxPath = part switch { 1 => 3, 2 => 10 };
            var minPath = part switch { 1 => 0, 2 => 4 };

            var input = File.ReadLines("Input17.txt").Select(s => s.Select(c => int.Parse(c.ToString())).ToList()).ToList();

            var visited = new Dictionary<Key, int>();
            var unvisited = new Dictionary<Key, int>
            {
                { new Key(new Coord(0, 0), Dir.E, 0), 0},
                { new Key(new Coord(0, 0), Dir.S, 0), 0},
            };

            while (unvisited.Count > 0)
            {
                var (key, score) = unvisited.MinBy(kvp => kvp.Value);

                unvisited.Remove(key);
                visited.Add(key, score);

                void Tentative(Key newKey)
                {
                    if (visited.ContainsKey(newKey))
                    {
                        return;
                    }

                    if (newKey.Pos.X < 0 || newKey.Pos.Y < 0 || newKey.Pos.X >= input.Count || newKey.Pos.Y >= input[0].Count)
                    {
                        return;
                    }

                    var newScore = score + input[newKey.Pos.X][newKey.Pos.Y];
                    if (!unvisited.TryGetValue(newKey, out var bestScore) || newScore < bestScore)
                    {
                        unvisited[newKey] = newScore;
                    }
                }

                if (key.Path < maxPath)
                {
                    Tentative(new Key(key.Pos.Move(key.Dir), key.Dir, key.Path + 1));
                }

                if (key.Path >= minPath)
                {
                    switch (key.Dir)
                    {
                        case Dir.N:
                        case Dir.S:
                            Tentative((new Key(key.Pos.Move(Dir.E), Dir.E, 1)));
                            Tentative((new Key(key.Pos.Move(Dir.W), Dir.W, 1)));
                            break;
                        case Dir.E:
                        case Dir.W:
                            Tentative((new Key(key.Pos.Move(Dir.N), Dir.N, 1)));
                            Tentative((new Key(key.Pos.Move(Dir.S), Dir.S, 1)));
                            break;
                    }
                }
            }

            var result = visited
                .Where(kvp => kvp.Key.Pos.X == input.Count - 1 && kvp.Key.Pos.Y == input[0].Count - 1 && kvp.Key.Path >= minPath)
                .Select(kvp => kvp.Value).Min();

            Console.WriteLine(result);
        }

        private record Key(Coord Pos, Dir Dir, int Path);
    }
}