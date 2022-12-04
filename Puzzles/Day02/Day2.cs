using System;
using System.Collections.Generic;

namespace AoC22;

public class Day2 : Puzzle
{
    private List<Turn> turns1 = new();
    private List<Turn> turns2 = new();

    public Day2(ILogger logger, string path) : base(logger, path) { }

    public override void Setup()
    {
        foreach (var line in Utils.ReadFrom(_path))
        {
            var opponentsHand = line[0] switch
            {
                'A' => Hand.Rock,
                'B' => Hand.Paper,
                'C' => Hand.Scissors,
                _ => throw new System.Exception(),
            };

            turns1.Add(new Turn(
                opponentsHand,
                line[2] switch
                {
                    'X' => Hand.Rock,
                    'Y' => Hand.Paper,
                    'Z' => Hand.Scissors,
                    _ => throw new System.Exception(),
                }
            ));

            turns2.Add(new Turn(
                opponentsHand,
                line[2] switch
                {
                    'X' => GetHandFor(opponentsHand, Result.Lose),
                    'Y' => GetHandFor(opponentsHand, Result.Draw),
                    'Z' => GetHandFor(opponentsHand, Result.Win),
                    _ => throw new System.Exception(),
                }
            ));
        }
    }

    private Hand GetHandFor(Hand opponent, Result result)
    {
        if (result == Result.Win)
            return opponent switch
            {
                Hand.Rock => Hand.Paper,
                Hand.Paper => Hand.Scissors,
                Hand.Scissors => Hand.Rock,
                _ => throw new Exception(),
            };

        if (result == Result.Draw)
            return opponent;

        if (result == Result.Lose)
            return opponent switch
            {
                Hand.Rock => Hand.Scissors,
                Hand.Paper => Hand.Rock,
                Hand.Scissors => Hand.Paper,
                _ => throw new Exception(),
            };

        throw new Exception();
    }

    public override void SolvePart1()
    {
        int score = CaalculateScore(turns1);
        _logger.Log(score);
    }

    public override void SolvePart2()
    {
        int score = CaalculateScore(turns2);
        _logger.Log(score);
    }

    private static int CaalculateScore(List<Turn> turns)
    {
        int score = 0;
        foreach (var turn in turns)
        {
            score += (int)turn.MyHand;

            score += (turn.OpponentsHand, turn.MyHand, turn.MyHand == turn.OpponentsHand) switch
            {
                (_, _, true) => 3,
                (Hand.Rock, Hand.Paper, _) => 6,
                (Hand.Paper, Hand.Scissors, _) => 6,
                (Hand.Scissors, Hand.Rock, _) => 6,
                _ => 0
            };
        }

        return score;
    }

    private record Turn(Hand OpponentsHand, Hand MyHand); 

    private enum Hand
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3,
    }

    private enum Result
    {
        Lose,
        Win,
        Draw,
    }
}
