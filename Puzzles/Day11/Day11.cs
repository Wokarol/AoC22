using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC22;

public partial class Day11 : Puzzle
{
    private MonkeyDefinition[] monkeyDefinitions;

    [GeneratedRegex(@"Monkey\s(?<id>\d):\s+.{16}(?<items>[\d, ]+)\s+.{21}(?<op>.) (?<opsec>.+)\s+.{19}(?<div>\d+)\s+.{25}(?<mt>\d+)\s+.{26}(?<mf>\d+)")]
    private partial Regex MonkeyGetterRegex();

    public Day11(ILogger logger, string path) : base(logger, path) { }

    public override void Setup()
    {
        string input = Utils.ReadAllText(_path);
        var matches = MonkeyGetterRegex().Matches(input) ?? throw new Exception();

        monkeyDefinitions = new MonkeyDefinition[matches.Count];
        for (int i = 0; i < matches.Count; i++)
        {
            Match match = matches[i];
            monkeyDefinitions[int.Parse(match.Groups["id"].ValueSpan)] = new()
            {
                StartingItems = match.Groups["items"].Value.Split(",").Select(v => int.Parse(v)).ToList(),
                Operation = match.Groups["op"].Value switch { "*" => Operation.Mult, "+" => Operation.Sum, _ => throw new Exception() },
                OperationSecondary = match.Groups["opsec"].Value.Contains("old") ? -1 : int.Parse(match.Groups["opsec"].Value),
                Divisor = int.Parse(match.Groups["div"].Value),
                MonkeyIfTrue = int.Parse(match.Groups["mt"].Value),
                MonkeyIfFalse = int.Parse(match.Groups["mf"].Value),
            };
        }
    }

    public override void SolvePart1()
    {
        //int r = 1;
        //foreach (var monkeys in Simulate().Take(20))
        //{
        //    _logger.Log($"\n\u001b[31mRound: {r}\u001b[0m");
        //    r++;
        //    for (int i = 0; i < monkeys.Length; i++)
        //    {
        //        _logger.Log($"Monkey {i}: {string.Join(", ", monkeys[i].Items)}");
        //    }
        //}

        var monkeyBusiness = Simulate(true)
            .Take(20)
            .Last()
            .Select(m => m.InspectionCount)
            .OrderByDescending(x => x)
            .Take(2)
            .Aggregate(1ul, (a, b) => a * b);

        _logger.Log(monkeyBusiness.ToString());
    }

    public override void SolvePart2()
    {
        var monkeyBusiness = Simulate(false)
            .Take(10000)
            .Last()
            .Select(m => m.InspectionCount)
            .OrderByDescending(x => x)
            .Take(2)
            .Aggregate(1ul, (a, b) => a * b);

        _logger.Log(monkeyBusiness.ToString());
    }

    private IEnumerable<Monkey[]> Simulate(bool worryDebuf)
    {
        Monkey[] monkeys = monkeyDefinitions
            .Select(def => new Monkey()
            {
                Properties = def,
                Items = def.StartingItems.Select(x => (ulong)x).ToList(),
            })
            .ToArray();

        while (true)
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.Items.Count > 0)
                {
                    var originalItem = monkey.Items[0];
                    monkey.Items.RemoveAt(0);

                    checked
                    {

                        var inspectedItem = monkey.ModifyItemWorry(originalItem);

                        var finalItem = worryDebuf
                            ? (ulong)(inspectedItem / 3.0)
                            : inspectedItem;

                        finalItem %= 232_792_560; // LCM of all numbers from 1 to 20

                        var passedCheck = monkey.TestItem(finalItem);
                        if (passedCheck)
                        {
                            monkeys[monkey.Properties.MonkeyIfTrue].Items.Add(finalItem);
                        }
                        else
                        {
                            monkeys[monkey.Properties.MonkeyIfFalse].Items.Add(finalItem);
                        }
                    }
                }
            }
            yield return monkeys;
        }
    }

    private class MonkeyDefinition
    {
        public List<int> StartingItems { get; init; }
        public Operation Operation { get; init; }
        public int OperationSecondary { get; init; }
        public int Divisor { get; init; }
        public int MonkeyIfTrue { get; init; }
        public int MonkeyIfFalse { get; init; }
    }

    private enum Operation
    {
        Mult,
        Sum,
    }

    private class Monkey
    {
        public List<ulong> Items { get; init; }
        public MonkeyDefinition Properties { get; init; }

        public ulong InspectionCount { get; private set; } = 0;

        public ulong ModifyItemWorry(ulong item)
        {
            InspectionCount++;

            checked
            {
                ulong secondValue = Properties.OperationSecondary == -1
                    ? item
                    : (ulong)Properties.OperationSecondary;

                return Properties.Operation switch
                {
                    Operation.Sum => item + secondValue,
                    Operation.Mult => item * secondValue,
                    _ => throw new Exception()
                };
            }
        }

        public bool TestItem(ulong item)
        {
            return item % (ulong)Properties.Divisor == 0;
        }
    }
}
