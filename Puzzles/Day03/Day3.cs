using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC22;

public class Day3 : Puzzle
{
    private string[] _data = Array.Empty<string>();

    public Day3(ILogger logger, string path) : base(logger, path) { }

    public override void Setup() => _data = Utils.ReadAllLines(_path);

    public override void SolvePart1()
    {
        int score = 0;
        foreach (var line in _data)
        {
            var half = line.Length >> 1;
            var charInCommon = CharsInCommon(line[..half], line[half..]).FirstOrDefault();
            score += ScoreForChar(charInCommon);
        }
        _logger.Log(score);
    }

    public override void SolvePart2()
    {
        int score = 0;
        for (int i = 0; i < _data.Length - 2; i += 3)
        {
            var charsInCommon = CharsInCommon(_data[i], _data[i + 1]);
            var charInCommon = CharsInCommon(charsInCommon, _data[i + 2]).FirstOrDefault();
            score += ScoreForChar(charInCommon);
        }
        _logger.Log(score);
    }

    private static IEnumerable<char> CharsInCommon(IEnumerable<char> a, IEnumerable<char> b) => a.Intersect(b);

    private static int ScoreForChar(char c)
    {
        if ('a' <= c && c <= 'z') return c - 96; // 'a' is 97, 'z' is 122. Returns 1-26
        if ('A' <= c && c <= 'Z') return c - 38; // 'A' is 65, 'Z' is 90.  Returns 27-52
        return 0;
    }
}