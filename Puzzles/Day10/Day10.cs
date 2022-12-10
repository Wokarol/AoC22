using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC22;

public partial class Day10 : Puzzle
{
    private List<Command> commands = new();

    public Day10(ILogger logger, string path) : base(logger, path) { }

    public override void Setup()
    {
        foreach (var line in Utils.ReadFrom(_path))
        {
            if (line == "noop")
            {
                commands.Add(Command.Noop);
            }
            else
            {
                int change = int.Parse(line[4..^0]);
                commands.Add(Command.Addx(change));
            }
        }
    }

    public override void SolvePart1()
    {
        int tick = 1;
        int sum = 0;
        foreach (var v in Simulate())
        {
            if ((tick - 20) % 40 == 0)
            {
                int signal = tick * v;
                sum += signal;
            }

            tick++;
        }
        _logger.Log($"{sum}");
    }

    public override void SolvePart2()
    {
        int tick = 1;
        char[] line = new char[40];
        int pos = 0;
        foreach (var v in Simulate())
        {
            bool isSpriteOnPixel = Math.Abs(v - pos) <= 1;
            line[pos] = isSpriteOnPixel ? '▒' : ' ';

            pos++;
            if (pos == line.Length)
            {
                _logger.Log(new string(line));
                pos = 0;
            }

            tick++;
        }
    }

    private IEnumerable<int> Simulate()
    {
        int x = 1;

        foreach (var command in commands)
        {
            if (!command.IsNoop)
            {
                yield return x;
                yield return x;
                x += command.Change;
            }
            else
            {
                yield return x;
            }
        }
    }

    public readonly struct Command
    {
        public readonly bool IsNoop;
        public readonly int Change;

        public Command(bool isNoop, int change)
        {
            IsNoop = isNoop;
            Change = change;
        }

        public static Command Noop => new(true, 0);
        public static Command Addx(int change) => new(false, change);
    }
}
