using System;
using System.Collections.Generic;

namespace AoC22;

public class Day1 : Puzzle
{
    private List<int> _data = new();
    
    public Day1(ILogger logger, string path) : base(logger, path) { }
    public Day1(string path) : base(path) { }

    public override void Setup()
    {
        foreach (var line in Utils.ReadFrom(_path))
        {
            Console.WriteLine(line);
            // convert line of data to something maleable
        }
    }

    public override void SolvePart1()
    {
        // TODO: magic
        _logger.Log("Answer for part 1 here");
    }

    public override void SolvePart2()
    {
        _logger.Log("Answer for part 2 here");
    }
}