using System.Collections.Generic;

namespace AoC22;

public class Day3 : Puzzle
{

    public Day3(ILogger logger, string path) : base(logger, path) { }

    public override void Setup()
    {
        foreach (var line in Utils.ReadFrom(_path, ignoreWhiteSpace: false))
        {

        }
    }

    public override void SolvePart1()
    {
        
        _logger.Log(420);
    }

    public override void SolvePart2()
    {
        _logger.Log(69);
    }
}