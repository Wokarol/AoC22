using System.Collections.Generic;

namespace AoC22;

public class Day1 : Puzzle
{
    // Example spot to store any parsed data
    // private List<int> _data = new();
    
    public Day1(ILogger logger, string path) : base(logger, path) { }

    public override void Setup()
    {
        foreach (var line in Utils.ReadFrom(_path))
        {
            // convert line of data to something usable
        }
    }

    public override void SolvePart1()
    {
        _logger.Log("Answer for part 1 here");
    }

    public override void SolvePart2()
    {
        _logger.Log("Answer for part 2 here");
    }
}