using System.Collections.Generic;
using System.Linq;

namespace AoC22;

public class Day1 : Puzzle
{
    // Example spot to store any parsed data
    // private List<int> _data = new();

    private List<int> elves = new();

    public Day1(ILogger logger, string path) : base(logger, path) { }

    public override void Setup()
    {
        elves.Add(0);
        foreach (var line in Utils.ReadFrom(_path))
        {
            if (string.IsNullOrEmpty(line))
            {
                elves.Add(0);
            }
            else
            {
                elves[^1] += int.Parse(line);
            }
        }
    }

    public override void SolvePart1()
    {
        _logger.Log(elves.Max());
    }

    public override void SolvePart2()
    {
        _logger.Log(elves.OrderByDescending(x => x).Take(3).Sum());
    }
}