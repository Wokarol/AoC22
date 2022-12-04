using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC22;

public class Day3 : Puzzle
{
    private List<Sack> sacks = new();

    public Day3(ILogger logger, string path) : base(logger, path) { }

    public override void Setup()
    {
        foreach (var line in Utils.ReadFrom(_path))
        {
            sacks.Add(new Sack(
                line[0..(line.Length / 2)],
                line[(line.Length / 2)..^0]
                ));
        }
    }

    public override void SolvePart1()
    {
        int priority = 0;

        foreach (var s in sacks)
        {
            var overlap = new HashSet<char>(s.Left);
            overlap.IntersectWith(s.Right);

            priority += overlap
                .Select(c => (c >= 'A' && c <= 'Z') ? ((c - 'A') + 27) : ((c - 'a') + 1))
                .Sum();
        }

        _logger.Log(priority);
    }

    public override void SolvePart2()
    {
        int priority = 0;

        foreach (var s in sacks.Chunk(3))
        {
            var overlap = new HashSet<char>(s[0].Left + s[0].Right);

            for (int i = 1; i < s.Length; i++)
            {
                var sack = s[i];
                overlap.IntersectWith(sack.Left + sack.Right);
            }

            if (overlap.Count != 1)
                throw new Exception();

            priority += overlap
                .Select(c => (c >= 'A' && c <= 'Z') ? ((c - 'A') + 27) : ((c - 'a') + 1))
                .Sum();
        }

        _logger.Log(priority);
    }

    private record Sack(string Left, string Right);
}
