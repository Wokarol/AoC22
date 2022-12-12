using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC22;

public class Day06 : Puzzle
{
    string datastream = "";

    public Day06(ILogger logger, string path) : base(logger, path) { }

    public override void Setup()
    {
        datastream = Utils.ReadFrom(_path).First();
    }

    public override void SolvePart1()
    {
        _logger.Log(GetMarker(4));
    }


    public override void SolvePart2()
    {
        _logger.Log(GetMarker(14));
    }

    private int GetMarker(int markerLength)
    {
        Span<int> mask = stackalloc int['z' - 'a' + 1];
        for (int i = 0; i < markerLength; i++)
        {
            mask[datastream[i] - 'a']++;
        }

        if (IsMaskWinning(mask))
            return markerLength;

        for (int i = markerLength; i < datastream.Length; i++)
        {
            mask[datastream[i - markerLength] - 'a']--;
            mask[datastream[i] - 'a']++;

            if (IsMaskWinning(mask))
                return i + 1;
        }

        return 0;
    }

    private bool IsMaskWinning(Span<int> mask)
    {
        for (int j = 0; j < mask.Length; j++)
        {
            if (mask[j] >= 2)
            {
                return false;
            }
        }

        return true;
    }
}
