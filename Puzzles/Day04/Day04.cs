using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC22;

public class Day04 : Puzzle
{
    private List<ElfPair> elfPairs = new();

    public Day04(ILogger logger, string path) : base(logger, path) { }

    public override void Setup()
    {
        foreach (var line in Utils.ReadFrom(_path))
        {
            var match = Regex.Match(line, @"(\d+)-(\d+),(\d+)-(\d+)");
            elfPairs.Add(new()
            {
                ElfAStart = int.Parse(match.Groups[1].ValueSpan),
                ElfAEnd = int.Parse(match.Groups[2].ValueSpan),
                ElfBStart = int.Parse(match.Groups[3].ValueSpan),
                ElfBEnd = int.Parse(match.Groups[4].ValueSpan),
            });
        }
    }

    public override void SolvePart1()
    {
        int count = 0;
        foreach (var p in elfPairs)
        {
            bool aContainsB = p.ElfAStart <= p.ElfBStart && p.ElfAEnd >= p.ElfBEnd;
            bool bContainsA = p.ElfBStart <= p.ElfAStart && p.ElfBEnd >= p.ElfAEnd;
            if (aContainsB || bContainsA)
                count++;
        }

        _logger.Log(count);
    }

    public override void SolvePart2()
    {
        int count = 0;
        foreach (var p in elfPairs)
        {
            bool aStartsInB = p.ElfAStart >= p.ElfBStart && p.ElfAStart <= p.ElfBEnd;
            bool BStartsInA = p.ElfBStart >= p.ElfAStart && p.ElfBStart <= p.ElfAEnd;
            if (aStartsInB || BStartsInA)
                count++;
        }

        _logger.Log(count);
    }

    private struct ElfPair
    {
        public int ElfAStart;
        public int ElfAEnd;
        public int ElfBStart;
        public int ElfBEnd;
    }
}
