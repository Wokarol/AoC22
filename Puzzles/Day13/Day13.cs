using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AoC22;

public partial class Day13 : Puzzle
{
    private List<string> packets;
    private PacketIterator firstIterator = new();
    private PacketIterator secondIterator = new();

    public Day13(ILogger logger, string path) : base(logger, path) { }

    public override void Setup()
    {
        packets = new(460);
        foreach (var line in Utils.ReadAllLines(_path))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            packets.Add(line);
        }
    }

    public override void SolvePart1()
    {
        int sum = 0;
        for (int i = 0; i < packets.Count; i += 2)
        {
            if (ComparePackets(packets[i], packets[i + 1]) == 1)
            {
                sum += (i / 2) + 1;
            }
        }
        _logger.Log(sum);
    }

    public override void SolvePart2()
    {
        packets.Add("[[2]]");
        packets.Add("[[6]]");

        packets.Sort(ReverseComparePackets);

        int firstDivider = packets.IndexOf("[[2]]") + 1;
        int secondDivider = packets.IndexOf("[[6]]") + 1;

        _logger.Log(firstDivider * secondDivider);
    }

    private int ReverseComparePackets(string first, string second) => -ComparePackets(first, second);

    private int ComparePackets(string first, string second)
    {
        firstIterator.ResetAndBind(first);
        secondIterator.ResetAndBind(second);
        while (true)
        {
            var firstToken = firstIterator.Iterate();
            var secondToken = secondIterator.Iterate();

            switch (firstToken, secondToken)
            {
                case (TokenType.Number, TokenType.Number):
                    // We have number comparision
                    if (firstIterator.Number != secondIterator.Number)
                    {
                        return firstIterator.Number < secondIterator.Number ? 1 : -1;
                    }
                    break;

                case (TokenType.Number, TokenType.ArrayStart):
                    firstIterator.PushFakeArray();
                    break;

                case (TokenType.ArrayStart, TokenType.Number):
                    secondIterator.PushFakeArray();
                    break;

                case (TokenType.ArrayStart, TokenType.ArrayStart):
                case (TokenType.ArrayEnd, TokenType.ArrayEnd):
                    // We start array or end on both
                    break;

                case (TokenType.End, TokenType.End):
                    // We ended the streams at the same time
                    return 0;

                case (TokenType.ArrayEnd, _):
                case (TokenType.End, _):
                    // The first one ended sooner
                    return 1;

                case (_, TokenType.ArrayEnd):
                case (_, TokenType.End):
                    // The second one ended sooner
                    return -1;

                default:
                    break;
            }
        }
    }

    class PacketIterator
    {
        private string packet;
        private int position;
        private int fakeArrayStack = 0;
        private bool shouldReemitLastToken = false;
        private TokenType lastToken;

        public int Number { get; private set; }

        public void ResetAndBind(string source)
        {
            packet = source;
            position = -1;
            fakeArrayStack = 0;
        }

        public TokenType Iterate()
        {
            if (fakeArrayStack != 0)
            {
                if (shouldReemitLastToken)
                {
                    shouldReemitLastToken = false;
                    return lastToken;
                }

                fakeArrayStack--;
                lastToken = TokenType.ArrayEnd;
                return TokenType.ArrayEnd;
            }

            while (true)
            {
                position++;

                if (position >= packet.Length)
                {
                    lastToken = TokenType.End;
                    return TokenType.End;
                }

                switch (packet[position])
                {
                    case '[':
                        lastToken = TokenType.ArrayStart;
                        return TokenType.ArrayStart;

                    case ']':
                        lastToken = TokenType.ArrayEnd;
                        return TokenType.ArrayEnd;
                }

                if (IsNum(packet[position]))
                {
                    while (IsNum(packet[position]))
                    {
                        int startPos = position;
                        while (IsNum(packet[position]))
                        {
                            position++;
                        }

                        var numStr = packet.AsSpan()[startPos..position];

                        Number = int.Parse(numStr);
                    }
                    position--;
                    lastToken = TokenType.Number;
                    return TokenType.Number;
                }
            }

            static bool IsNum(char c)
            {
                return c >= '0' && c <= '9';
            }
        }

        public void PushFakeArray()
        {
            fakeArrayStack++;
            shouldReemitLastToken = true;
        }
    }
    enum TokenType
    {
        ArrayStart,
        ArrayEnd,
        Number,
        End,
    }
}
