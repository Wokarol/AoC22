using System.Collections.Generic;
using System.Linq;

namespace AoC22;

public class Day1 : Puzzle
{
    private readonly List<int> _data = new();

    public Day1(ILogger logger, string path) : base(logger, path) { }

    public override void Setup()
    {
        _data.Clear();
        int current = 0;
        foreach (var line in Utils.ReadFrom(_path))
        {
            if (string.IsNullOrEmpty(line))
            {
                if (current != 0) _data.Add(current);
                current = 0;
                continue;
            }
            if (int.TryParse(line, out var calories)) current += calories;
        }
        if (current != 0) _data.Add(current); // in case input.txt doesn't end on a newline
    }

    public override void SolvePart1() => _logger.Log(_data.Max());

    public override void SolvePart2() => _logger.Log(_data.OrderDescending().Take(3).Sum());
}