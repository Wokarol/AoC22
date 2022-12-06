using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC22;

public class Day6 : Puzzle
{
    string datastream = "";

    public Day6(ILogger logger, string path) : base(logger, path) { }

    public override void Setup()
    {
        datastream = Utils.ReadFrom(_path).First();
    }

    public override void SolvePart1()
    {
        int marker = 0;
        const int markerLength = 4;
        for (int i = markerLength; i <= datastream.Length; i++)
        {
            if (datastream[(i - markerLength)..i].Distinct().Count() == markerLength)
            {
                marker = i;
                break;
            }
        }
        _logger.Log(marker);
    }

    public override void SolvePart2()
    {
        int marker = 0;
        const int markerLength = 14;
        for (int i = markerLength; i <= datastream.Length; i++)
        {
            if (datastream[(i - markerLength)..i].Distinct().Count() == markerLength)
            {
                marker = i;
                break;
            }
        }
        _logger.Log(marker);
    }
}
