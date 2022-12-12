using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC22;

public class Day05 : Puzzle
{
    Stack<char>[] stacks;
    List<Move> moves = new();


    public Day05(ILogger logger, string path) : base(logger, path) { }

    public override void Setup()
    {
        List<string> containerDefinitions = new();

        bool isFirstSection = true;

        foreach (var line in Utils.ReadFrom(_path))
        {
            if (isFirstSection)
            {
                if (string.IsNullOrEmpty(line))
                {
                    isFirstSection = false;
                }
                else
                {
                    containerDefinitions.Add(line);
                }
            }
            else
            {
                var match = Regex.Match(line, @"move (\d+) from (\d+) to (\d+)");
                moves.Add(new Move()
                {
                    Count = int.Parse(match.Groups[1].ValueSpan),
                    From = int.Parse(match.Groups[2].ValueSpan) - 1,
                    To = int.Parse(match.Groups[3].ValueSpan) - 1,
                });
            }
        }

        stacks = Enumerable.Range(0, (containerDefinitions.Last().Length + 1) / 4)
            .Select(i => new Stack<char>())
            .ToArray();


        foreach (var def in containerDefinitions.AsEnumerable().Reverse().Skip(1))
        {
            for (int n = 0; n < stacks.Length; n++)
            {
                var c = def[4 * n + 1];
                if (c != ' ')
                    stacks[n].Push(c);
            }
        }
    }

    public override void SolvePart1()
    {
        var stacksCopy = new Stack<char>[stacks.Length];
        for (int i = 0; i < stacksCopy.Length; i++)
        {
            stacksCopy[i] = new Stack<char>(stacks[i].Reverse());
        }

        foreach (var move in moves)
        {
            var from = stacksCopy[move.From];
            var to = stacksCopy[move.To];
            for (int i = 0; i < move.Count; i++)
            {
                to.Push(from.Pop());
            }
        }

        _logger.Log(string.Join("", stacksCopy.Select(s => s.Peek())));
    }

    public override void SolvePart2()
    {

        var stacksCopy = new Stack<char>[stacks.Length];
        for (int i = 0; i < stacksCopy.Length; i++)
        {
            stacksCopy[i] = new Stack<char>(stacks[i].Reverse());
        }

        foreach (var move in moves)
        {
            var from = stacksCopy[move.From];
            var to = stacksCopy[move.To];

            var taken = Enumerable.Range(0, move.Count)
                .Select(x => from.Pop())
                .Reverse();

            foreach (var x in taken)
            {
                to.Push(x);
            }
        }

        _logger.Log(string.Join("", stacksCopy.Select(s => s.Peek())));
    }

    public struct Move
    {
        public int From;
        public int To;
        public int Count;
    }
}
