using System.Collections.Generic;

namespace AoC22;

public class Day2 : Puzzle
{
    private readonly Dictionary<string, int> _data = new();

    private const char ROCK = 'X';
    private const char PAPER = 'Y';
    private const char SCISSORS = 'Z';

    public Day2(ILogger logger, string path) : base(logger, path) { }

    public override void Setup()
    {
        foreach (var line in Utils.ReadFrom(_path))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            _data.AddToOrCreate(line, 1); // just keeping track of number of occurances for each matchup
        }
    }

    public override void SolvePart1()
    {
        var score = 0;
        foreach (var kvp in _data)
            score += kvp.Value * (PointsForThrowing(kvp.Key[2]) + PointsForResult(kvp.Key));
        _logger.Log(score);
    }

    // X/Y/Z are our throws. A/B/C are opponent throws (not used here).
    private static int PointsForThrowing(char c) => c switch
    {
        ROCK => 1,
        PAPER => 2,
        SCISSORS => 3,
        _ => 0,
    };

    private static int PointsForResult(string matchup) => matchup switch
    {
        "A Z" or "B X" or "C Y" => 0, // Loss
        "A X" or "B Y" or "C Z" => 3, // Tie
        "A Y" or "B Z" or "C X" => 6, // Win
        _ => 0,
    };

    public override void SolvePart2()
    {
        var score = 0;
        foreach (var kvp in _data)
            score += kvp.Value * (PointsForThrowing(UpdatedThrow(kvp.Key)) + PointsForKnownResult(kvp.Key[2]));
        _logger.Log(score);
    }

    // Updated rules: X = need to lose, Y = tie, Z = win. This returns the value of what we need to *throw* to meet the condition
    private static char UpdatedThrow(string matchup) => matchup switch
    {
        "A Y" or "B X" or "C Z" => ROCK, // Tie (Y) their Rock (A), Lose (X) to Paper (B), or Win (Z) against Scissors (C)
        "A Z" or "B Y" or "C X" => PAPER, // Beats (Z) Rock (A), Ties (Y) Paper (B), Loses (X) to Scissors (C)
        "A X" or "B Z" or "C Y" => SCISSORS, // Loses (X) to Rock (A), Beats (Z) Paper (B), Ties (Y) Scissors (C)
        _ => '\0',
    };

    // X/Y/Z now means Loss/Tie/Win so we just remap the point values to match: 1/2/3 -> 0/3/6
    private static int PointsForKnownResult(char c) => 3 * (PointsForThrowing(c) - 1);
}
// nice