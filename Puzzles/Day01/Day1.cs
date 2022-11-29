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
        }
    }

    public override void SolvePart1()
    {
        base.SolvePart1();
    }

    public override void SolvePart2()
    {
        base.SolvePart2();
    }
}