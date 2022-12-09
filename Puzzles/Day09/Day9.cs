using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC22;

public partial class Day9 : Puzzle
{
    private List<Command> commands = new();

    public Day9(ILogger logger, string path) : base(logger, path) { }

    public override void Setup()
    {
        foreach (var line in Utils.ReadFrom(_path))
        {
            var match = ParseCommandRegex().Match(line);
            var dir = match.Groups[1].Value switch
            {
                "U" => Direction.Up,
                "D" => Direction.Down,
                "L" => Direction.Left,
                "R" => Direction.Right,
                _ => throw new Exception()
            };
            var count = int.Parse(match.Groups[2].ValueSpan);

            commands.Add(new(dir, count));
        }
    }

    public override void SolvePart1()
    {
        (int x, int y) head = (0, 0);
        (int x, int y) tail = head;
        HashSet<(int, int)> visited = new();

        foreach (var command in commands)
        {
            for (int i = 0; i < command.Count; i++)
            {
                (int x, int y) headDelta = DeltaFromDirection(command.Dir);
                head = (head.x + headDelta.x, head.y + headDelta.y);

                tail = MoveTail(tail, head);

                visited.Add(tail);
            }
        }
        _logger.Log(visited.Count);
    }

    public override void SolvePart2()
    {
        (int x, int y)[] segments = new (int, int)[10];
        HashSet<(int, int)> visited = new();

        foreach (var command in commands)
        {
            for (int i = 0; i < command.Count; i++)
            {
                (int x, int y) headDelta = DeltaFromDirection(command.Dir);
                segments[0] = (segments[0].x + headDelta.x, segments[0].y + headDelta.y);

                for (int s = 1; s < segments.Length; s++)
                {
                    segments[s] = MoveTail(segments[s], segments[s - 1]);
                }

                visited.Add(segments[^1]);
            }
        }
        _logger.Log(visited.Count);
    }

    [GeneratedRegex(@"^(\w) (\d+)$")]
    private partial Regex ParseCommandRegex();

    private struct Command
    {
        public Direction Dir;
        public int Count;

        public Command(Direction dir, int count)
        {
            Dir = dir;
            Count = count;
        }
    }

    private enum Direction { Left, Right, Up, Down }

    private (int x, int y) DeltaFromDirection(Direction d) => d switch
    {
        Direction.Up => (0, 1),
        Direction.Down => (0, -1),
        Direction.Right => (1, 0),
        Direction.Left => (-1, 0),
        _ => throw new Exception()
    };

    private (int x, int y) MoveTail((int x, int y) tail, (int x, int y) head)
    {
        (int x, int y) tailDelta = (0, 0);
        if (Math.Abs(tail.x - head.x) > 1 || Math.Abs(tail.y - head.y) > 1)
        {
            int deltaX = head.x - tail.x;
            int deltaY = head.y - tail.y;
            tailDelta = (Math.Sign(deltaX), Math.Sign(deltaY));
        }

        return (tail.x + tailDelta.x, tail.y + tailDelta.y);
    }
}
